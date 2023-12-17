using System;

namespace FSM.Editor.Function
{
    public class FunctionNode : NodeWithConnection
    {
        private object functionObject;

        public FunctionNode(object functionObject) : base(functionObject.GetType().Name)
        {
            
        }

        public Type GetReturningType() => functionObject.GetType().GenericTypeArguments[0];

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