using FSM.Runtime;

namespace FSM.Editor
{
    public abstract class ConditionalNode : Node
    {
        protected ConditionalNode(string nodeName) : base(nodeName)
        {
        }
        protected ConditionalNode(IConditionalLayoutNode node) : base(node.GetType().Name)
        {
        }
    }
}