using System;
using UnityEngine;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateTransitionModel
    {
        public Vector2Model AnchorNodePosition;
        public string SourceName;
        public string TargetName;
        public TransitionContextModel ContextModel;

        public StateTransitionModel()
        {
        }

        public StateTransitionModel(Vector2 anchorNodePosition, string sourceName, string targetName, TransitionContextModel contextModel)
        {
            AnchorNodePosition = (Vector2Model)anchorNodePosition;
            SourceName = sourceName;
            TargetName = targetName;
            ContextModel = contextModel;
        }
    }
}