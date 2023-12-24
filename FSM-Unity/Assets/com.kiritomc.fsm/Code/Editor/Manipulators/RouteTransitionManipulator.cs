using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSM.Editor.Events;
using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class RouteTransitionManipulator<TNode> : PointerManipulator where TNode: VisualStateNode
    {
        private readonly VisualStateNode currentNode;
        private bool isRouting;

        private readonly StatesContext context;
        private readonly List<VisualElement> buffer = new List<VisualElement>(10);
        private bool isPressed;
        private Vector2 lastMousePosition;

        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public RouteTransitionManipulator(VisualStateNode currentNode, StatesContext context)
        {
            this.currentNode = currentNode;
            this.context = context;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            currentNode.RegisterCallback<PointerDownEvent>(StartTransitionRouting);
            context.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.RegisterCallback<TransitionRequestEvent<TNode>>(HandleTransitionRequest);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            currentNode.UnregisterCallback<PointerDownEvent>(StartTransitionRouting);
            context.RegisterCallback<MouseUpEvent>(HandleMouseUp);
            target.UnregisterCallback<TransitionRequestEvent<TNode>>(HandleTransitionRequest);
        }

        private async void StartTransitionRouting(PointerDownEvent e)
        {
            if (isRouting) return;
            isRouting = true;
            if (e.button == 1)
            {
                VisualStateNode targetNode = await TransitionRequest.NewAsync(currentNode).Invoke();
                if (CheckValid(targetNode))
                {
                    VisualStateTransition transition = new VisualStateTransition(currentNode, targetNode);
                    currentNode.Add(transition);
                    currentNode.Transitions.Add(transition);
                }
                bool CheckValid<T>(T stateNode) where T: VisualStateNode
                {
                    return targetNode != null 
                           && targetNode != currentNode 
                           && currentNode.Transitions.All(item => item.Target != stateNode);
                }
            }
            isRouting = false;
        }

        private void HandleMouseUp(MouseUpEvent e)
        {
            if (e.button != 1) return;
            isPressed = false;
            lastMousePosition = e.mousePosition;
        }

        private async void HandleTransitionRequest(TransitionRequestEvent<TNode> e)
        {
            isPressed = true;
            EditorState.DraggingLocked.Value = true;
            while (isPressed) await Task.Yield();
            EditorState.DraggingLocked.Value = false;

            TNode node = context.panel.Pick<TNode>(lastMousePosition, buffer);
            e.SetTransitionCallback.Invoke(node);
            e.SetTransitionCallback = default;
        }
    }
}