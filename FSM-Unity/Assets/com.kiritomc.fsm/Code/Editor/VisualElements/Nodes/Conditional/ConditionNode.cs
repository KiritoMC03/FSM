using FSM.Runtime;

namespace FSM.Editor
{
    public class ConditionNode : ConditionalNode
    {
        public ConditionNode(ConditionLayoutNode node) : base(node.LogicObject.GetType().Name)
        {
        }

        public override string GetMetadataForSerialization()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            throw new System.NotImplementedException();
        }
    }
}