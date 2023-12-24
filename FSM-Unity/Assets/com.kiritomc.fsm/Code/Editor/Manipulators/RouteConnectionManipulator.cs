using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class RouteConnectionManipulator : Manipulator
    {
        private readonly VisualElement mousePressTrackingElement;
        private readonly List<VisualElement> buffer = new List<VisualElement>(10);
        private bool isPressed;
        private Vector2 lastMousePosition;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public RouteConnectionManipulator(VisualElement mousePressTrackingElement)
        {
            this.mousePressTrackingElement = mousePressTrackingElement;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            mousePressTrackingElement.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.RegisterCallback<VisualNodeLinkRequestEvent>(HandleConnectionRequest);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            mousePressTrackingElement.UnregisterCallback<MouseUpEvent>(HandleMouseUp);
            target.UnregisterCallback<VisualNodeLinkRequestEvent>(HandleConnectionRequest);
        }

        private void HandleMouseUp(MouseUpEvent e)
        {
            if (e.button != 0) return;
            isPressed = false;
            lastMousePosition = e.mousePosition;
        }

        private async void HandleConnectionRequest(VisualNodeLinkRequestEvent e)
        {
            isPressed = true;
            EditorState.DraggingLocked.Value = true;
            while (isPressed) await Task.Yield();
            EditorState.DraggingLocked.Value = false;

            mousePressTrackingElement.panel.PickAll(lastMousePosition, buffer);
            foreach (VisualElement element in buffer)
                if (element is IVisualNodeWithLinkExit node)
                {
                    e.TargetGotCallback.Invoke(node);
                    e.TargetGotCallback = default;
                    return;
                }
        }
    }
}