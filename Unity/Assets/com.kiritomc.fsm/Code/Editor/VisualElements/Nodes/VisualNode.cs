using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNode : VisualElement, ICustomRepaintHandler
    {
        public string Name;
        public VisualElement Header;
        public TextElement Label;
        public readonly int Id;
        private readonly List<VisualNodeStyleRegistration> styleRegistrations = new List<VisualNodeStyleRegistration>();

        protected VisualNodeStyleRegistration BaseStyleRegistration;

        public Vector2 ResolvedPlacement => new Vector2(resolvedStyle.left, resolvedStyle.top);
        public Vector2 Placement
        {
            get => new Vector2(style.left.value.value, style.top.value.value);
            set
            {
                style.left = value.x;
                style.top = value.y;
            }
        }

        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public void AppendStyleRegistration(VisualNodeStyleRegistration registration)
        {
            styleRegistrations.Add(registration);
            Repaint();
        }

        public VisualNode(int id)
        {
            Id = id;
            BaseStyleRegistration = new VisualNodeStyleRegistration(this, () =>
            {
                style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = Sizes.NodePadding;
                style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = Colors.NodeBorderColor;
                style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = Sizes.NodeBorderWidth;
                style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = Sizes.NodeBorderRadius;
                style.position = new StyleEnum<Position>(Position.Absolute);
                style.minWidth = 200;
                style.minHeight = 50;
                style.backgroundColor = Colors.NodeBackground;
            });
        }

        public VisualNode(string nodeName, int id) : this(id)
        {
            Name = nodeName;
            Header = this.NodeHeader();
            Label = Header.NodeLabel($"{nodeName} ({id})");
        }

        public virtual void Repaint()
        {
            for (int i = 0; i < styleRegistrations.Count; i++)
            {
                VisualNodeStyleRegistration current = styleRegistrations[i];
                if (current.IsDisposed)
                {
                    styleRegistrations.RemoveAt(i);
                    i--;
                }
                else current.Apply();
            }
        }

        public NodeChangingListeningRegistration OnChanged(Action action) => new NodeChangingListeningRegistration(this, action);
    }
}