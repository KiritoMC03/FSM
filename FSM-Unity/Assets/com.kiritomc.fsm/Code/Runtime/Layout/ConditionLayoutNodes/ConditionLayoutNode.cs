namespace FSM.Runtime
{
    public sealed class ConditionLayoutNode : IConditionalLayoutNode
    {
        public ICondition LogicObject { get; set; }

        public ConditionLayoutNode()
        {
        }

        public ConditionLayoutNode(ICondition conditionObject)
        {
            LogicObject = conditionObject;
        }
    }
}