namespace FSM.Runtime
{
    public sealed class AndLayoutNode : ConditionGateLayoutNode, IConditionalLayoutNodeWithRightBranch
    {
        public IConditionalLayoutNode Right { get; set; }

        public AndLayoutNode() : base()
        {
        }

        public AndLayoutNode(IConditionalLayoutNode left, IConditionalLayoutNode right) : base(left)
        {
            Right = right;
        }
    }
}