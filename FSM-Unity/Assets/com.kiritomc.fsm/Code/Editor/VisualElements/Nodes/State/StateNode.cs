using UnityEngine;

namespace FSM.Editor
{
    public class StateNode : Node
    {
        protected string StateName;

        public StateNode(string nodeName) : base(nodeName)
        {
            StateName = nodeName;
        }

        public override string GetMetadataForSerialization()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetNearestAbsoluteEdgePoint(Vector2 target)
        {
            throw new System.NotImplementedException();
        }
    }
}