namespace FSM.Runtime
{
    public class ConditionGateLayoutNode : IConditionalLayoutNode, IConditionalLayoutNodeWithLeftBranch
    {
        public IConditionalLayoutNode Left { get; set; }

        public ConditionGateLayoutNode()
        {
        }

        public ConditionGateLayoutNode(IConditionalLayoutNode left)
        {
            Left = left;
        }
    }
}