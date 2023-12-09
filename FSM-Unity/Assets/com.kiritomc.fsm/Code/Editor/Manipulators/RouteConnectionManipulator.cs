using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class RouteConnectionManipulator : Manipulator
    {
        private readonly EditorState editorState;
        private readonly VisualElement mousePressTrackingElement;
        private readonly List<VisualElement> buffer = new List<VisualElement>(10);
        private bool isPressed;
        private Vector2 lastMousePosition;

        public RouteConnectionManipulator(EditorState editorState, VisualElement mousePressTrackingElement)
        {
            this.editorState = editorState;
            this.mousePressTrackingElement = mousePressTrackingElement;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            mousePressTrackingElement.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.RegisterCallback<ConnectionRequestEvent>(HandleConnectionRequest);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            mousePressTrackingElement.UnregisterCallback<MouseUpEvent>(HandleMouseUp);
            target.UnregisterCallback<ConnectionRequestEvent>(HandleConnectionRequest);
        }

        private void HandleMouseUp(MouseUpEvent e)
        {
            isPressed = false;
            lastMousePosition = e.mousePosition;
        }

        private async void HandleConnectionRequest(ConnectionRequestEvent e)
        {
            isPressed = true;
            editorState.DraggingLocked.Value = true;
            while (isPressed) await Task.Yield();
            editorState.DraggingLocked.Value = false;

            mousePressTrackingElement.panel.PickAll(lastMousePosition, buffer);
            foreach (VisualElement element in buffer)
                if (element is Node node)
                {
                    e.SetConnectionCallback.Invoke(node);
                    e.SetConnectionCallback = default;
                    return;
                }
        }
    }
}