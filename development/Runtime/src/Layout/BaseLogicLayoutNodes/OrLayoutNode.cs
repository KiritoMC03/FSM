namespace FSM.Runtime
{
    public sealed class OrLayoutNode : BaseGateNode
    {
        public ILayoutNode Right { get; }

        public OrLayoutNode(ILayoutNode left, ILayoutNode right) : base(default, left)
        {
            Right = right;
        }
    }
}