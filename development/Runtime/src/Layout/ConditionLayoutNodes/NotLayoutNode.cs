namespace FSM.Runtime
{
    public sealed class NotLayoutNode : ConditionGateLayoutNode
    {
        public NotLayoutNode() : base()
        {
        }
        
        public NotLayoutNode(IConditionalLayoutNode left) : base(left)
        {
        }
    }
}