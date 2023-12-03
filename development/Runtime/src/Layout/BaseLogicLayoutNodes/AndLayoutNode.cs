namespace FSM.Runtime
{
    public sealed class AndLayoutNode : BaseGateNode
    {
        public ILayoutNode Right { get; }

        public AndLayoutNode(ILayoutNode left, ILayoutNode right) : base(default, left)
        {
            Right = right;
        }
    }
}