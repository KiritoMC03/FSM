namespace FSM.Runtime
{
    public class ConditionGateLayoutNode : IConditionalLayoutNode
    {
        public IConditionalLayoutNode Left { get; }

        public ConditionGateLayoutNode(IConditionalLayoutNode left)
        {
            Left = left;
        }
    }
}