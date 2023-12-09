using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class Modifiers
    {
        public static bool DraggingLocked = false;

        public static void AsDraggable(this VisualElement element)
        {
            bool isPressed = false;
            Vector2 offset = default;
            element.RegisterCallback<MouseLeaveEvent>(StopDrag);
            element.RegisterCallback<MouseUpEvent>(StopDrag);
            element.RegisterCallback<MouseDownEvent>(HandleMouseDown);
            element.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
            return;

            void StartDrag() => isPressed = true;
            void StopDrag<T>(T _) => isPressed = false;
            void HandleMouseDown(IMouseEvent evt)
            {
                if (DraggingLocked) return;
                StartDrag();
                element.BringToFront();
                offset = new Vector2(
                    element.resolvedStyle.left - evt.mousePosition.x,
                    element.resolvedStyle.top - evt.mousePosition.y);
            }
            void HandleMouseMove(IMouseEvent evt)
            {
                if (!isPressed) return;
                if (element is ICustomRepaintHandler repaintHandler) repaintHandler.Repaint();
                element.style.left = evt.mousePosition.x + offset.x;
                element.style.top = evt.mousePosition.y + offset.y;
            }
        }

        public static LineDrawerRegistration AddLineDrawerForConnection<T>(this Node target, Func<Vector2?> startGetter, Func<T> targetFieldGetter) 
            where T: Node
        {
            LineDrawer drawer;
            target.Add(drawer = new LineDrawer());
            return new LineDrawerRegistration(drawer, target, startGetter, GetEndPosition);
            Vector2? GetEndPosition() => targetFieldGetter.Invoke()?.GetAbsoluteConnectionPos();
        }

        public static LineDrawerRegistration AddLineDrawerFor(this Node parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
        {
            LineDrawer drawer;
            parent.Add(drawer = new LineDrawer());
            return new LineDrawerRegistration(drawer, parent, startGetter, endGetter);
        }

        public static NodeConnectionField BindAsConnectionField<T>(this Node node, 
            string fieldName, 
            FieldWrapper<T> fieldWrapper,
            Func<Task<Node>> asyncTargetGetter) where T : Node
        {
            NodeConnectionField connectionField = NodeConnectionField.Create($"{fieldName}");
            LineDrawerRegistration lineDrawerRegistration = node.AddLineDrawerForConnection(GetInputPosition, GetFieldValue);
            NodeChangingListeningRegistration changingRegistration = default;
            connectionField.OnMouseDown += async _ =>
            {
                Node target = await asyncTargetGetter.Invoke();
                if (target is T correct)
                {
                    fieldWrapper.Value = correct;
                    RepaintNode();
                    changingRegistration?.Dispose();
                    changingRegistration = correct.ListenChanges(RepaintNode);
                }
            };

            node.ChildrenRepaintHandler.Add(lineDrawerRegistration);
            node.Disposables.Add(lineDrawerRegistration);
            node.Add(connectionField);
            return connectionField;

            Vector2? GetInputPosition() => connectionField.AnchorCenter();
            T GetFieldValue() => fieldWrapper.Value;
            void RepaintNode() => node.Repaint();
        }

        public static T AddGet<T>(this IList<T> list, T element)
        {
            list.Add(element);
            return element;
        }

        public static void Repaint(this IEnumerable<ICustomRepaintHandler> list)
        {
            foreach (ICustomRepaintHandler handler in list) handler.Repaint();
        }

        public static NodeChangingListeningRegistration ListenChanges(this Node node, Action onChanged)
        {
            if (node == null || onChanged == null) return default;
            return new NodeChangingListeningRegistration(node, onChanged);
        }
    }
}