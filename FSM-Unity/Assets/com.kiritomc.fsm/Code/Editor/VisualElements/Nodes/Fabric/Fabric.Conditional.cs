using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public partial class Fabric
    {
        public NotNode ConditionalNotNode(TransitionContext transitionContext, Vector2 position = default)
        {
            NotNode node = new NotNode(new NotLayoutNode());
            MakeDefaults(node, transitionContext, position);
            return node;
        }

        public OrNode ConditionalOrNode(TransitionContext transitionContext, Vector2 position = default)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            MakeDefaults(node, transitionContext, position);
            return node;
        }

        public AndNode ConditionalAndNode(TransitionContext transitionContext, Vector2 position = default)
        {
            AndNode node = new AndNode(new AndLayoutNode());
            MakeDefaults(node, transitionContext, position);
            return node;
        }

        public ConditionNode ConditionalConditionNode(TransitionContext transitionContext, ConditionLayoutNode conditionLayoutNode, Vector2 position = default)
        {
            ConditionNode node = new ConditionNode(conditionLayoutNode);
            node.style.left = position.x;
            node.style.top = position.y;
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            transitionContext.Add(node);
            return node;
        }

        private void MakeDefaults(ConditionalNode node, TransitionContext transitionContext, Vector2 position)
        {
            node.style.left = position.x;
            node.style.top = position.y;
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(EditorState, transitionContext));
            transitionContext.Add(node);
        }
    }
}