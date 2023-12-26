using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class TransitionContextModel
    {
        public VisualNodeModel[] ConditionalNodeModels;

        public TransitionContextModel()
        {
        }

        public TransitionContextModel(VisualNodeModel[] conditionalNodeModels)
        {
            ConditionalNodeModels = conditionalNodeModels;
        }
    }
}