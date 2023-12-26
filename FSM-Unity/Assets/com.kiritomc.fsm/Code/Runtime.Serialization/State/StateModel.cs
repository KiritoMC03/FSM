using System;
using System.Collections.Generic;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class StateModel
    {
        public string Name;
        public Dictionary<string, AbstractSerializableType<ConditionalLayoutNodeModel>> Transitions;

        public StateModel()
        {
        }

        public StateModel(string name, Dictionary<string, AbstractSerializableType<ConditionalLayoutNodeModel>> transitions)
        {
            Name = name;
            Transitions = transitions;
        }
    }
}