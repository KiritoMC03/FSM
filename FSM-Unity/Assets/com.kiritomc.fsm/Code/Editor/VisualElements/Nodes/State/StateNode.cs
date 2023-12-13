using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSM.Editor.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StateNode : Node
    {
        public readonly string StateName;
        public List<StateTransition> Transitions;

        public StateNode(string nodeName, Vector2 position, IEnumerable<StateTransition> transitions = default) : base(nodeName)
        {
            StateName = nodeName;
            style.left = position.x;
            style.top = position.y;
            Transitions = new List<StateTransition>(transitions ?? ArraySegment<StateTransition>.Empty);
            RegisterCallback<PointerDownEvent>(async e =>
            {
                if (e.button == 1)
                {
                    StateTransition transition = await Fabric.Instance.RouteTransitionAsync(this, RequestTransition, CheckValid);
                    if (transition != null) Transitions.Add(transition);
                    bool CheckValid(StateNode stateNode) => Transitions.All(item => item.Target != stateNode);
                }
            });
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            return base.ContainsPoint(localPoint);
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
            Vector2 size = new Vector2(resolvedStyle.width / 2f, resolvedStyle.height / 2f);
            Vector2 dir = target - worldBound.center;
            Vector2 edge = PointOnBoundsA(size, dir.normalized);
            Vector2 PointOnBoundsA(Vector2 bounds, Vector2 direction)
            {
                float y = bounds.x * direction.y / direction.x;
                float xSign = Mathf.Sign(direction.x);
                float ySign = Mathf.Sign(direction.y);
                if (Mathf.Abs(y) < bounds.y)
                {
                    return new Vector2(bounds.x * xSign, xSign < 0 ? -y : y);
                }
                return new Vector2(bounds.y * direction.x / direction.y * ySign, bounds.y * ySign);
            }
            return worldBound.center + edge;
        }

        public override void Repaint()
        {
            base.Repaint();
            foreach (StateTransition transition in Transitions) transition.SendToBack();
        }
    }
}