using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateNodeLifecycleModel
    {
        public Vector2Model AnchorNodePosition;
        public int OnEnterId = -1;
        public int OnUpdateId = -1;
        public int OnExitId = -1;
        public VisualNodeModel[] Nodes;

        public StateNodeLifecycleModel()
        {
        }

        public StateNodeLifecycleModel(Vector2Model anchorNodePosition, int onEnterId, int onUpdateId, int onExitId, VisualNodeModel[] nodes)
        {
            AnchorNodePosition = anchorNodePosition;
            OnEnterId = onEnterId;
            OnUpdateId = onUpdateId;
            OnExitId = onExitId;
            Nodes = nodes;
        }
    }
}