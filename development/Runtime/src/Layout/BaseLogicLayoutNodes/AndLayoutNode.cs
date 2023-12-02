namespace FSM.Runtime
{
    public sealed class AndLayoutNode : BaseLogicGateNode
    {
        public ILogicLayoutNode Right { get; }

        public AndLayoutNode(ILogicLayoutNode input, ILogicLayoutNode right) : base(input, default)
        {
            Right = right;
        }
    }
}