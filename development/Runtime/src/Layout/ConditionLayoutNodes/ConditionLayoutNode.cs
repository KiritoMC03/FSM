namespace FSM.Runtime
{
    public sealed class ConditionLayoutNode : IConditionalLayoutNode
    {
        public ICondition LogicObject { get; }

        public ConditionLayoutNode(ICondition conditionObject)
        {
            LogicObject = conditionObject;
        }
    }
}