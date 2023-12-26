using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class TransitionContextModel
    {
        public int ConditionAnchorId;
        public VisualNodeModel[] ConditionalNodeModels;

        public TransitionContextModel()
        {
        }

        public TransitionContextModel(int conditionAnchorId, VisualNodeModel[] conditionalNodeModels)
        {
            ConditionAnchorId = conditionAnchorId;
            ConditionalNodeModels = conditionalNodeModels;
        }
    }
}