using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Extensions;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using PlasticGui;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class Fabric
    {
        public static Fabric Instance { get; private set; } 
        private readonly EditorState editorState;
        private VisualElement root;

        public Fabric(EditorState editorState, VisualElement root)
        {
            Instance = this;
            this.editorState = editorState;
            this.root = root;
        }
        
        public OrNode TestConditional(VisualElement pointerTrackingElement)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(editorState, pointerTrackingElement));
            return node;
        }

        public SelectNodePopup CreateSelectNodePopup(IEnumerable<string> availableNodes, Action<string> selectedHandler)
        {
            SelectNodePopup result = new SelectNodePopup(availableNodes, selectedHandler);
            root.Add(result);
            Vector2 position = result.WorldToLocal(editorState.PointerPosition.Value);
            result.style.position = Position.Absolute;
            result.style.left = position.x;
            result.style.top = position.y;
            return result;
        }

        public StateNode CreateStateNode(string name, VisualElement pointerTrackingElement, Vector2 position = default)
        {
            StateNode node = new StateNode(name, position);
            node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
            node.AddManipulator(new CreateTransitionManipulator(editorState, pointerTrackingElement));
            return node;
        }

        #region Transitions

        public async Task<StateTransition> RouteTransitionAsync<T>(T source, Func<Task<T>> asyncTargetGetter, Predicate<T> checkValid)
            where T : StateNode
        {
            T target = await asyncTargetGetter.Invoke();
            if (target == null || target == source || !checkValid.Invoke(target)) return default;
            return CreateTransition(source, target);
        }

        public StateTransition CreateTransition<T>(T source, T target)
            where T : StateNode
        {
            StateTransition transition = new StateTransition(source, target);
            source.Add(transition);
            source.Disposables.Add(transition);
            source.Disposables.Add(target.ListenChanges(RepaintTransition));
            source.ChildrenRepaintHandler.Add(transition);
            return transition;
            void RepaintTransition() => transition.Repaint();
        }

        #endregion
    }
}