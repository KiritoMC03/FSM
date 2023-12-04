namespace FSM.Runtime
{
    public sealed class NotLayoutNode : ConditionGateLayoutNode
    {
        public NotLayoutNode(IConditionalLayoutNode left) : base(left)
        {
        }
    }
}