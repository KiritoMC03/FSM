using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class TransitionContextModel
    {
        public TransitionContextEntryNodeModel[] ConditionalNodeModels;

        public TransitionContextModel()
        {
        }

        public TransitionContextModel(TransitionContextEntryNodeModel[] conditionalNodeModels)
        {
            ConditionalNodeModels = conditionalNodeModels;
        }
    }
}