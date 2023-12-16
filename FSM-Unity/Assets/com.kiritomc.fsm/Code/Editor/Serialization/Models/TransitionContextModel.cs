using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class TransitionContextModel
    {
        public ConditionalNodeModel[] ConditionalNodeModels;

        public TransitionContextModel()
        {
        }

        public TransitionContextModel(ConditionalNodeModel[] conditionalNodeModels)
        {
            ConditionalNodeModels = conditionalNodeModels;
        }
    }
}