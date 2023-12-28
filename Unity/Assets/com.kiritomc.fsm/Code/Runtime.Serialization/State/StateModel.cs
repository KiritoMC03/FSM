using System;
using System.Collections.Generic;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class StateModel
    {
        public string Name;
        public Dictionary<string, AbstractSerializableType<ConditionalLayoutNodeModel>> Transitions;
        public AbstractSerializableType<ActionLayoutNodeModel> OnEnter;
        public AbstractSerializableType<ActionLayoutNodeModel> OnUpdate;
        public AbstractSerializableType<ActionLayoutNodeModel> OnExit;

        public StateModel()
        {
        }

        public StateModel(
            string name, 
            Dictionary<string, 
            AbstractSerializableType<ConditionalLayoutNodeModel>> transitions, 
            AbstractSerializableType<ActionLayoutNodeModel> onEnter, 
            AbstractSerializableType<ActionLayoutNodeModel> onUpdate, 
            AbstractSerializableType<ActionLayoutNodeModel> onExit)
        {
            Name = name;
            Transitions = transitions;
            OnEnter = onEnter;
            OnUpdate = onUpdate;
            OnExit = onExit;
        }
    }
}