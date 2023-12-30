using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class ScaleContextManipulator<T> : Manipulator
        where T: VisualNode
    {
        private const float Sensitivity = 0.02f;
        private readonly VisualNodesContext<T> context;
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public ScaleContextManipulator(VisualNodesContext<T> context)
        {
            this.context = context;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<WheelEvent>(HandleMouseWheel);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.RegisterCallback<WheelEvent>(HandleMouseWheel);
        }

        private void HandleMouseWheel(WheelEvent e)
        {
            context.ScaleContent(-e.delta.y * Sensitivity);
        }
    }

    public class ContextDraggingManipulator<T> : Manipulator
        where T: VisualNode
    {
        private readonly VisualNodesContext<T> context;
        private readonly int mouseButton;
        private bool isPressed;
        private Vector2 prevPressPosition;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public ContextDraggingManipulator(VisualNodesContext<T> context, int mouseButton = 2)
        {
            this.context = context;
            this.mouseButton = mouseButton;
        }

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
            if (e.button != mouseButton) return;
            if (EditorState.DraggingLocked.Value) return;
            isPressed = true;
            target.BringToFront();
            prevPressPosition = new Vector2(e.position.x, e.position.y);
        }

        private void HandleMouseMove(PointerMoveEvent e)
        {
            if (!isPressed) return;
            if (EditorState.DraggingLocked.Value) return;
            Vector2 currentPressPosition = new Vector2(e.position.x, e.position.y);
            Vector2 offset = currentPressPosition - prevPressPosition;
            context.MoveNodes(offset);
            if (target is ICustomRepaintHandler repaintHandler) repaintHandler.Repaint();
            prevPressPosition = currentPressPosition;
        }

        private void StopDrag<TEvent>(TEvent e) where TEvent: IPointerEvent
        {
            if (e.button == mouseButton)
                isPressed = false;
        }
    }
}