namespace FSM.Runtime
{
    public sealed class OrLayoutNode : ConditionGateLayoutNode, IConditionalLayoutNodeWithRightBranch
    {
        public IConditionalLayoutNode Right { get; set; }

        public OrLayoutNode() : base()
        {
        }

        public OrLayoutNode(IConditionalLayoutNode left, IConditionalLayoutNode right) : base(left)
        {
            Right = right;
        }
    }
}