using FSM.Runtime;

namespace FSM.Editor
{
    public class ConditionNode : ConditionalNode
    {
        public ConditionNode(ConditionLayoutNode node) : base(node.LogicObject.GetType().Name)
        {
        }
    }
}