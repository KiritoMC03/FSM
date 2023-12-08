using FSM.Runtime;

namespace FSM.Editor
{
    public class AndNode : ConditionalNode
    {
        public ConditionalNode Left;
        public ConditionalNode Right;

        public AndNode(AndLayoutNode node) : base(node)
        {
            Add(NodeConnectionField.Create($"{nameof(Left)}"));
            Add(NodeConnectionField.Create($"{nameof(Right)}"));
        }
    }
}