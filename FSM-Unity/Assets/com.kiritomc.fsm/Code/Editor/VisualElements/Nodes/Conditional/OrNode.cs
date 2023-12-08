using FSM.Runtime;

namespace FSM.Editor
{
    public class OrNode : ConditionalNode
    {
        public ConditionalNode Left;
        public ConditionalNode Right;

        public OrNode(OrLayoutNode node) : base(node)
        {
            Add(NodeConnectionField.Create($"{nameof(Left)}"));
            Add(NodeConnectionField.Create($"{nameof(Right)}"));
        }
    }
}