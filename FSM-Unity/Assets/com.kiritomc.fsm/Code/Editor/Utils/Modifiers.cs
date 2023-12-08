using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public static class Modifiers
    {
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
    }
}