using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class StateNodeLifecycleModel
    {
        public Vector2Model AnchorNodePosition;
        public StateNodeLifecycleNodeModel OnEnter;
        public StateNodeLifecycleNodeModel OnUpdate;
        public StateNodeLifecycleNodeModel OnExit;
        public VisualNodeModel[] Nodes;

        public StateNodeLifecycleModel()
        {
        }

        public StateNodeLifecycleModel(Vector2Model anchorNodePosition, StateNodeLifecycleNodeModel onEnter, StateNodeLifecycleNodeModel onUpdate, StateNodeLifecycleNodeModel onExit, VisualNodeModel[] nodes)
        {
            AnchorNodePosition = anchorNodePosition;
            OnEnter = onEnter;
            OnUpdate = onUpdate;
            OnExit = onExit;
            Nodes = nodes;
        }
    }

    [Serializable]
    public class StateNodeLifecycleNodeModel
    {
        public int SelfId = -1;
        public StateNodeLifecycleNodeModel Linked;

        public StateNodeLifecycleNodeModel()
        {
        }

        public StateNodeLifecycleNodeModel(int selfId, StateNodeLifecycleNodeModel linked)
        {
            SelfId = selfId;
            Linked = linked;
        }
    }
}