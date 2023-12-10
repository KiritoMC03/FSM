using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class CreateTransitionManipulator : Manipulator
    {
        private readonly EditorState editorState;
        private readonly VisualElement mousePressTrackingElement;
        private readonly List<VisualElement> buffer = new List<VisualElement>(10);
        private bool isPressed;
        private Vector2 lastMousePosition;

        public CreateTransitionManipulator(EditorState editorState, VisualElement mousePressTrackingElement)
        {
            this.editorState = editorState;
            this.mousePressTrackingElement = mousePressTrackingElement;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            mousePressTrackingElement.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.RegisterCallback<TransitionRequestEvent>(HandleTransitionRequest);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            mousePressTrackingElement.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.UnregisterCallback<TransitionRequestEvent>(HandleTransitionRequest);
        }

        private void HandleMouseUp(MouseUpEvent e)
        {
            if (e.button != 1) return;
            isPressed = false;
            lastMousePosition = e.mousePosition;
        }

        private async void HandleTransitionRequest(TransitionRequestEvent e)
        {
            isPressed = true;
            editorState.DraggingLocked.Value = true;
            while (isPressed) await Task.Yield();
            editorState.DraggingLocked.Value = false;

            mousePressTrackingElement.panel.PickAll(lastMousePosition, buffer);
            foreach (VisualElement element in buffer)
                if (element is StateNode node)
                {
                    e.SetTransitionCallback.Invoke(node);
                    e.SetTransitionCallback = default;
                    return;
                }
        }
    }
}