using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class RouteVisualNodeLinkManipulator : Manipulator
    {
        private readonly VisualElement mousePressTrackingElement;
        private readonly List<VisualElement> buffer = new List<VisualElement>(10);
        private bool isPressed;
        private Vector2 upMousePosition;
        private Vector2 pressedMousePosition;
        private VisualNodeLinkDrawerRegistration tempLineDrawerRegistration;
        private Vector2 startedPosition;

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public RouteVisualNodeLinkManipulator(VisualElement mousePressTrackingElement)
        {
            this.mousePressTrackingElement = mousePressTrackingElement;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            tempLineDrawerRegistration = new VisualNodeLinkDrawerRegistration(mousePressTrackingElement, GetStartPosition, GetEndPosition);
            mousePressTrackingElement.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.RegisterCallback<VisualNodeLinkRequestEvent>(HandleConnectionRequest);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            tempLineDrawerRegistration?.Dispose();
            tempLineDrawerRegistration = default;
            mousePressTrackingElement.UnregisterCallback<MouseUpEvent>(HandleMouseUp);
            target.UnregisterCallback<VisualNodeLinkRequestEvent>(HandleConnectionRequest);
        }

        private void HandleMouseUp(MouseUpEvent e)
        {
            if (e.button != 0 || !isPressed) return;
            isPressed = false;
            upMousePosition = e.mousePosition;
        }

        private void HandleMouseMove(MouseMoveEvent e)
        {
            pressedMousePosition = e.mousePosition;
        }

        private async void HandleConnectionRequest(VisualNodeLinkRequestEvent e)
        {
            pressedMousePosition = startedPosition = Event.current.mousePosition;
            isPressed = true;
            EditorState.DraggingLocked.Value = true;
            mousePressTrackingElement.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
            while (isPressed)
            {
                VisualNodeWithLinkExit current = Pick(pressedMousePosition);
                tempLineDrawerRegistration.Drawer.OverrideGradient = current != null ? e.TargetMatchCallback(current) ? default : Colors.NodeLinkErrorGradient : default;
                tempLineDrawerRegistration.Repaint();
                await Task.Yield();
            }
            EditorState.DraggingLocked.Value = false;
            mousePressTrackingElement.UnregisterCallback<MouseMoveEvent>(HandleMouseMove);
            tempLineDrawerRegistration.Clear();

            mousePressTrackingElement.panel.PickAll(upMousePosition, buffer);
            VisualNodeWithLinkExit result = Pick(upMousePosition);
            e.TargetGotCallback.Invoke(result);
            e.TargetGotCallback = default;
        }

        private Vector2? GetStartPosition()
        {
            return !isPressed ? default : mousePressTrackingElement.WorldToLocal(startedPosition);
        }

        private Vector2? GetEndPosition()
        {
            VisualNodeWithLinkExit result = Pick(pressedMousePosition);
            if (isPressed)
            {
                Vector2 position = pressedMousePosition;
                if (result != null && result != target) position = result.GetAbsoluteLinkPointPos();
                return position;
            }
            return default;
        }

        private VisualNodeWithLinkExit Pick(Vector2 position)
        {
            mousePressTrackingElement.panel.PickAll(position, buffer);
            foreach (VisualElement element in buffer)
                if (element is VisualNodeWithLinkExit node)
                    return node;
            return default;
        }
    }
}