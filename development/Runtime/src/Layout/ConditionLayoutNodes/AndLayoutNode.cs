namespace FSM.Runtime
{
    public sealed class AndLayoutNode : ConditionGateLayoutNode
    {
        public IConditionalLayoutNode Right { get; }

        public AndLayoutNode(IConditionalLayoutNode left, IConditionalLayoutNode right) : base(left)
        {
            Right = right;
        }
    }
}