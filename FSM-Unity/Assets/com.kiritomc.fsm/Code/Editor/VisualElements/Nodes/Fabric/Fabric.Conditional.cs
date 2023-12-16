using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public partial class Fabric
    {
        public NotNode ConditionalNotNode(TransitionContext transitionContext)
        {
            NotNode node = new NotNode(new NotLayoutNode());
            MakeDefaults(node, transitionContext);
            return node;
        }

        public OrNode ConditionalOrNode(TransitionContext transitionContext)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            MakeDefaults(node, transitionContext);
            return node;
        }

        public AndNode ConditionalAndNode(TransitionContext transitionContext)
        {
            AndNode node = new AndNode(new AndLayoutNode());
            MakeDefaults(node, transitionContext);
            return node;
        }

        public ConditionNode ConditionalConditionNode(TransitionContext transitionContext, ConditionLayoutNode conditionLayoutNode)
        {
            ConditionNode node = new ConditionNode(conditionLayoutNode);
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            transitionContext.Add(node);
            return node;
        }

        private void MakeDefaults(ConditionalNode node, TransitionContext transitionContext)
        {
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(EditorState, transitionContext));
            transitionContext.Add(node);
        }
    }
}