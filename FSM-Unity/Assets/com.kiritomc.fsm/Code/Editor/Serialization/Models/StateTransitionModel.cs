using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateTransitionModel
    {
        public string SourceName;
        public string TargetName;
        public TransitionContextModel ContextModel;

        public StateTransitionModel()
        {
        }

        public StateTransitionModel(string sourceName, string targetName, TransitionContextModel contextModel)
        {
            SourceName = sourceName;
            TargetName = targetName;
            ContextModel = contextModel;
        }
    }
}