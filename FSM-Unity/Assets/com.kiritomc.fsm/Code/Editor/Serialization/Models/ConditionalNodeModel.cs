using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class ConditionalNodeModel
    {
        public string Name;
        public Vector2Model Position;
        public string NodeKind;
        public int LeftConnectionId;
        public int RightConnectionId;

        public ConditionalNodeModel()
        {
        }

        public ConditionalNodeModel(string name, Vector2Model position, string nodeKind, int leftConnectionId, int rightConnectionId)
        {
            Name = name;
            Position = position;
            NodeKind = nodeKind;
            LeftConnectionId = leftConnectionId;
            RightConnectionId = rightConnectionId;
        }
    }
}