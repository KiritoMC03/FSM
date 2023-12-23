using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class DraggerManipulator: PointerManipulator
    {
        private bool isPressed;
        private Vector2 offset;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(HandleMouseDown);
            target.RegisterCallback<PointerMoveEvent>(HandleMouseMove);
            target.RegisterCallback<PointerUpEvent>(StopDrag);
            target.RegisterCallback<PointerLeaveEvent>(StopDrag);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(HandleMouseDown);
            target.UnregisterCallback<PointerMoveEvent>(HandleMouseMove);
            target.UnregisterCallback<PointerUpEvent>(StopDrag);
            target.UnregisterCallback<PointerLeaveEvent>(StopDrag);
        }

        private void HandleMouseDown(PointerDownEvent e)
        {
            if (EditorState.DraggingLocked.Value) return;
            isPressed = true;
            target.BringToFront();
            offset = new Vector2(
                target.resolvedStyle.left - e.position.x,
                target.resolvedStyle.top - e.position.y);
        }

        private void HandleMouseMove(PointerMoveEvent e)
        {
            if (!isPressed) return;
            if (EditorState.DraggingLocked.Value) return;
            if (target is ICustomRepaintHandler repaintHandler) repaintHandler.Repaint();
            target.style.left = e.position.x + offset.x;
            target.style.top = e.position.y + offset.y;
        }

        private void StopDrag<T>(T _) => isPressed = false;
    }
}