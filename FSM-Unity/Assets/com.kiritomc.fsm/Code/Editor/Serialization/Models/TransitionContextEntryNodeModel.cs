using System;
using System.Collections.Generic;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class TransitionContextEntryNodeModel
    {
        public Type Type;
        public Vector2Model Position;
        public Dictionary<string, int> Linked;

        public TransitionContextEntryNodeModel()
        {
        }

        public TransitionContextEntryNodeModel(Type type, Vector2Model position, Dictionary<string, int> linked)
        {
            Type = type;
            Position = position;
            Linked = linked;
        }

        public enum NodeKind
        {
            Condition,
            FunctionBool,
        }
    }
}