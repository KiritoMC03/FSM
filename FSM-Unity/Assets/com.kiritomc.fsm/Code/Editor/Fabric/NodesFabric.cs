using System;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodesFabric
    {
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        #region State

        public StateNode CreateStateNode(string name, StatesContext statesContext, Vector2 position = default)
        {
            StateNode node = new StateNode(name, position);
            node.AddManipulator(new StateNodeLabelManipulator(node, EditorState.DraggingLocked, changed =>
            {
                string newName = changed.newValue;
                int num = 1;
                if (!statesContext.StateNodes.Exists(i => i.Name == newName)) return newName;
                while (statesContext.StateNodes.Exists(i => i.Name == $"{newName} {num}")) num++;
                return $"{newName} {num}";
            }));
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            node.AddManipulator(new CreateTransitionManipulator(EditorState, statesContext));
            return node;
        }

        #endregion

        #region Conditional

        public ConditionNode ConditionalConditionNode(TransitionContext transitionContext, ConditionLayoutNode conditionLayoutNode, Vector2 position = default)
        {
            ConditionNode node = new ConditionNode(conditionLayoutNode);
            node.style.left = position.x;
            node.style.top = position.y;
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            transitionContext.Add(node);
            return node;
        }

        public NotNode ConditionalNotNode(TransitionContext transitionContext, Vector2 position = default) => SetupSpecialConditionalNode(() => new NotNode(new NotLayoutNode()), transitionContext, position);
        public OrNode ConditionalOrNode(TransitionContext transitionContext, Vector2 position = default) => SetupSpecialConditionalNode(() => new OrNode(new OrLayoutNode()), transitionContext, position);
        public AndNode ConditionalAndNode(TransitionContext transitionContext, Vector2 position = default) => SetupSpecialConditionalNode(() => new AndNode(new AndLayoutNode()), transitionContext, position);

        private T SetupSpecialConditionalNode<T>(Func<T> getter, TransitionContext transitionContext, Vector2 position) 
            where T: ConditionalNode
        {
            T node = getter();
            node.style.left = position.x;
            node.style.top = position.y;
            node.AsDraggable(EditorState);
            node.WithConnectionManipulator(transitionContext, EditorState);
            transitionContext.Add(node);
            return node;
        }

        #endregion
    }

    public static class NodesFabricExtensions
    {
        public static void AsDraggable(this Node node, EditorState editorState) => node.AddManipulator(new DraggerManipulator(editorState.DraggingLocked));
        public static void WithConnectionManipulator(this Node node, TransitionContext context, EditorState editorState) => node.AddManipulator(new RouteConnectionManipulator(editorState.DraggingLocked, context));
    }
}