using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Events;
using FSM.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StateNode : Node
    {
        protected string StateName;
        private readonly List<StateTransition> transitions = new List<StateTransition>();

        public StateNode(string nodeName) : base(nodeName)
        {
            StateName = nodeName;
            RegisterCallback<PointerDownEvent>(async e =>
            {
                if (e.button == 1)
                {
                    StateTransition transition = await this.CrateTransitionAsync(RequestTransition);
                    if (transition != null) transitions.Add(transition);
                }
            });
        }

        protected virtual Task<StateNode> RequestTransition()
        {
            TaskCompletionSource<StateNode> completionSource = new TaskCompletionSource<StateNode>();
            TransitionRequestEvent @event = TransitionRequestEvent.GetPooled();
            @event.target = this;
            @event.SetTransitionCallback = node => completionSource.SetResult(node);
            SendEvent(@event);
            return completionSource.Task;
        }

        public override string GetMetadataForSerialization()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetNearestAbsoluteEdgePoint(Vector2 target)
        {
            return worldBound.center;
        }

        public override void Repaint()
        {
            base.Repaint();
            foreach (StateTransition transition in transitions) transition.SendToBack();
        }
    }
}