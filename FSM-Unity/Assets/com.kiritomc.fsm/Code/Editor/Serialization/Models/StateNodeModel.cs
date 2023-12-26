using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateNodeModel
    {
        public string Name;
        public int Id;
        public Vector2Model Position;
        public StateTransitionModel[] OutgoingTransitions;
        public StateNodeLifecycleModel Lifecycle;

        public StateNodeModel()
        {
        }

        public StateNodeModel(string name, int id, Vector2Model position, StateTransitionModel[] outgoingTransitions, StateNodeLifecycleModel lifecycle)
        {
            Name = name;
            Id = id;
            Position = position;
            OutgoingTransitions = outgoingTransitions;
            Lifecycle = lifecycle;
        }
    }
}