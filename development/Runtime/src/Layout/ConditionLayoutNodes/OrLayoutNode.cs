namespace FSM.Runtime
{
    public sealed class OrLayoutNode : ConditionGateLayoutNode
    {
        public IConditionalLayoutNode Right { get; }

        public OrLayoutNode(IConditionalLayoutNode left, IConditionalLayoutNode right) : base(left)
        {
            Right = right;
        }
    }
}