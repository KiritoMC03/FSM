using FSM.Editor.Extensions;
using FSM.Runtime;
using Newtonsoft.Json;

namespace FSM.Editor
{
    public class ConditionGateNode : ConditionalNode
    {
        public FieldWrapper<ConditionalNode> Left = new FieldWrapper<ConditionalNode>();
        public FieldWrapper<ConditionalNode> Right = new FieldWrapper<ConditionalNode>();

        public ConditionGateNode(ConditionGateLayoutNode node) : base(node)
        {
            this.BindConnectionField(nameof(Left), Left, RequestConnection);
            this.BindConnectionField(nameof(Right), Right, RequestConnection);
        }

        public override string GetMetadataForSerialization()
        {
            return JsonConvert.SerializeObject((Left, Right));
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata)) return;
            (Left.Value, Right.Value) = JsonConvert.DeserializeObject<(ConditionalNode, ConditionalNode)>(metadata);
        }
    }
}