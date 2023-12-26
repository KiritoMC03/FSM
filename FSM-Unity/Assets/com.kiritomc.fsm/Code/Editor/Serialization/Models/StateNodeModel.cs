using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateNodeModel
    {
        public string Name;
        public Vector2Model Position;
        public StateTransitionModel[] OutgoingTransitions;
        public StateNodeLifecycleModel Lifecycle;

        public StateNodeModel()
        {
        }

        public StateNodeModel(string name, Vector2Model position, StateTransitionModel[] outgoingTransitions, StateNodeLifecycleModel lifecycle)
        {
            Name = name;
            Position = position;
            OutgoingTransitions = outgoingTransitions;
            Lifecycle = lifecycle;
        }
    }
}