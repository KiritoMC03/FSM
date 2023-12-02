namespace FSM.Runtime
{
    public sealed class OrLayoutNode : BaseLogicGateNode
    {
        public ILogicLayoutNode Right { get; }

        public OrLayoutNode(ILogicLayoutNode input, ILogicLayoutNode right) : base(input, default)
        {
            Right = right;
        }
    }
}