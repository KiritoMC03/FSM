using FSM.Runtime;
using Newtonsoft.Json;

namespace FSM.Editor
{
    public class NotNode : ConditionalNode
    {
        public FieldWrapper<ConditionalNode> Input = new FieldWrapper<ConditionalNode>();

        public NotNode(NotLayoutNode node) : base(node)
        {
            BuildConnectionField(nameof(Input), Input, RequestConnection);
        }

        public override string GetMetadataForSerialization()
        {
            return JsonConvert.SerializeObject(Input);
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata)) return;
            Input.Value = JsonConvert.DeserializeObject<ConditionalNode>(metadata);
        }
    }
}