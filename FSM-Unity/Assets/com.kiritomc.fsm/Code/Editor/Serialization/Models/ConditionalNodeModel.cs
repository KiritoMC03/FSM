using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class ConditionalNodeModel
    {
        public string Name;
        public Vector2Model Position;
        public string NodeKind;
        public string LeftConnectionName;
        public string RightConnectionName;

        public ConditionalNodeModel()
        {
        }

        public ConditionalNodeModel(string name, Vector2Model position, string nodeKind, string leftConnectionName, string rightConnectionName)
        {
            Name = name;
            Position = position;
            NodeKind = nodeKind;
            LeftConnectionName = leftConnectionName;
            RightConnectionName = rightConnectionName;
        }
    }
}