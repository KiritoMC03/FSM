using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNode : VisualElement, ICustomRepaintHandler
    {
        public string Name;
        public VisualElement Header;
        public TextElement Label;
        private readonly List<VisualNodeStyleRegistration> styleRegistrations = new List<VisualNodeStyleRegistration>();

        protected VisualNodeStyleRegistration BaseStyleRegistration;

        public Vector2 ResolvedPlacement => new Vector2(resolvedStyle.left, resolvedStyle.top);
        public Vector2 Placement => new Vector2(style.left.value.value, style.top.value.value);
        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public void AppendStyleRegistration(VisualNodeStyleRegistration registration) => styleRegistrations.Add(registration);

        public VisualNode()
        {
            styleRegistrations.Add(BaseStyleRegistration = new VisualNodeStyleRegistration(this, () =>
            {
                style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = Sizes.NodePadding;
                style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = Colors.NodeBorderColor;
                style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = Sizes.NodeBorderWidth;
                style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = Sizes.NodeBorderRadius;
                style.position = new StyleEnum<Position>(Position.Absolute);
                style.minWidth = 200;
                style.minHeight = 50;
                style.backgroundColor = Colors.NodeBackground;
            }));
        }

        public VisualNode(string nodeName) : this()
        {
            Name = nodeName;
            Header = this.NodeHeader();
            Label = Header.NodeLabel(nodeName);
        }

        public void Repaint()
        {
            for (int i = 0; i < styleRegistrations.Count; i++)
            {
                VisualNodeStyleRegistration current = styleRegistrations[i];
                if (current.IsDisposed) i--;
                else current.Apply();
            }
        }
    }

    public abstract class VisualNodeWithLinkExit : VisualNode
    {
        protected NodeLinkPoint Link;

        protected VisualNodeWithLinkExit()
        {
            Link = LinkPointRelativeTo(Header, Label, ConnectionPosition.Right);
        }

        public virtual Vector2 GetAbsoluteConnectionPos()
        {
            return new Vector2(Link.worldBound.x + Sizes.ConnectionNodePoint / 2f, Link.worldBound.y + Sizes.ConnectionNodePoint / 2f);
        }

        public static NodeLinkPoint LinkPointRelativeTo(VisualElement parent, VisualElement relative, ConnectionPosition position)
        {
            NodeLinkPoint link;
            parent.Add(link = new NodeLinkPoint());
            switch (position)
            {
                case ConnectionPosition.Left:
                    link.PlaceBehind(relative);
                    break;
                case ConnectionPosition.Right:
                    link.PlaceInFront(relative);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
            return link;
        }
    }

    public class VisualStateNode : VisualNode, IVisualNodeWithTransitions
    {
        public readonly TextInputBaseField<string> LabelInputField;

        public List<VisualStateTransition> Transitions { get; set; } = new List<VisualStateTransition>();

        public VisualStateNode(string stateName, StatesContext context, Vector2 position = default) : base(stateName)
        {
            LabelInputField = Header.NodeInputLabel(stateName);
            LabelInputField.style.display = DisplayStyle.None;
            style.left = position.x;
            style.top = position.y;
            this.AddManipulator(new RouteTransitionManipulator<VisualStateNode>(this, context));
        }

        public void RemoveTransitionAt(int index)
        {
            VisualStateTransition transition = Transitions[index];
            Transitions.RemoveAt(index);
            Fabric.Transitions.DestroyTransition(this, transition);
        }
    }

    // visual
    // manipulators +
    // elements

    public class VisualNodeStyleRegistration : IDisposable
    {
        private readonly VisualNode target;
        private readonly Action applyStyle;

        public bool IsDisposed { get; private set; }

        public VisualNodeStyleRegistration(VisualNode target, Action applyStyle)
        {
            this.target = target;
            this.applyStyle = applyStyle;
        }

        public void Dispose() => IsDisposed = true;

        public void Apply()
        {
            if (!IsDisposed) applyStyle();
        }
    }

    public class VisualNodeLinkRegistration : IDisposable
    {
        private readonly VisualNode parent;
        private readonly Func<Task<VisualNodeWithLinkExit>> asyncTargetGetter;
        private readonly Action<VisualNodeWithLinkExit> gotHandler;
        private readonly Func<VisualNodeWithLinkExit> currentGetter;
        private readonly NodeLinkFieldView linkFieldView;
        private readonly Subscription connectionFieldViewMouseDownSubscription;
        private readonly VisualNodeLinkDrawerRegistration linkDrawerRegistration;

        public bool IsDisposed { get; private set; }

        public VisualNodeLinkRegistration(
            VisualNode parent,
            string linkName,
            Func<Task<VisualNodeWithLinkExit>> asyncTargetGetter,
            Action<VisualNodeWithLinkExit> gotHandler,
            Func<VisualNodeWithLinkExit> currentGetter)
        {
            this.parent = parent;
            this.asyncTargetGetter = asyncTargetGetter;
            this.gotHandler = gotHandler;
            this.currentGetter = currentGetter;
            linkFieldView = new NodeLinkFieldView(linkName);
            connectionFieldViewMouseDownSubscription = linkFieldView.SubscribeMouseDown(ConnectionFieldViewMouseDownHandler);
            linkDrawerRegistration = new VisualNodeLinkDrawerRegistration(parent, GetLinkStart, GetLinkEnd);

            parent.Add(linkFieldView);
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.Remove(linkFieldView);
            connectionFieldViewMouseDownSubscription?.Dispose();
            linkDrawerRegistration?.Dispose();
            IsDisposed = true;
        }

        private async void ConnectionFieldViewMouseDownHandler(MouseDownEvent _)
        {
            VisualNodeWithLinkExit result = await asyncTargetGetter();
            gotHandler(result);
        }

        private Vector2? GetLinkStart() => linkFieldView?.AnchorCenter();
        private Vector2? GetLinkEnd() => currentGetter()?.GetAbsoluteConnectionPos();
    }

    public class VisualNodeLinkDrawerRegistration : IDisposable, ICustomRepaintHandler
    {
        private readonly VisualElement parent;
        private readonly Func<Vector2?> localStartGetter;
        private readonly Func<Vector2?> worldEndGetter;

        public NodeConnectionDrawer Drawer { get; private set; }
        public bool IsDisposed { get; private set; }

        public VisualNodeLinkDrawerRegistration(VisualElement parent, Func<Vector2?> localStartGetter, Func<Vector2?> worldEndGetter)
        {
            this.parent = parent;
            this.localStartGetter = localStartGetter;
            this.worldEndGetter = worldEndGetter;
            this.parent.Add(Drawer = new NodeConnectionDrawer());
            parent.generateVisualContent += Redraw;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            parent.generateVisualContent -= Redraw;
            parent.Remove(Drawer);
            IsDisposed = true;
        }

        private void Redraw(MeshGenerationContext meshGenerationContext)
        {
            Vector2? start = localStartGetter.Invoke();
            Vector2? end = worldEndGetter.Invoke();
            if (start.HasValue) Drawer.LocalStartOffset = start.Value;
            if (end.HasValue) Drawer.WorldEndPos = end.Value;
        }

        public void Repaint()
        {
            Drawer.MarkDirtyRepaint();
        }
    }

    public static class VisualNodeBuilder
    {
        public static VisualElement NodeHeader<T>(this T node)
            where T : VisualNode
        {
            VisualElement header = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            node.Add(header);
            return header;
        }


        public static Label NodeLabel<T>(this T nodeWithHeader, string text) where T : VisualNode
        {
            return nodeWithHeader.Header.NodeLabel(text);
        }

        public static Label NodeLabel(this VisualElement parent, string text)
        {
            Label label;
            parent.Add(label = new Label(text)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            });
            return label;
        }


        public static TextInputBaseField<string> NodeInputLabel<T>(this T nodeWithHeader, string text) where T : VisualNode
        {
            return nodeWithHeader.Header.NodeInputLabel(text);
        }

        public static TextInputBaseField<string> NodeInputLabel(this VisualElement parent, string text)
        {
            TextField field = new TextField()
            {
                value = text,
                style =
                {
                    alignContent = Align.Center,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            };
            parent.Add(field);
            return field;
        }
    }
}