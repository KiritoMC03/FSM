using System;

namespace FSM.Editor.Serialization
{
    public class StatesContextModel
    {
        public string Name;
        public StateNodeModel[] StateNodeModels = Array.Empty<StateNodeModel>();

        public StatesContextModel()
        {
        }

        public StatesContextModel(StateNodeModel[] stateNodeModels, string name)
        {
            StateNodeModels = stateNodeModels;
            Name = name;
        }
    }
}