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
        public List<StateTransition> Transitions;
        public readonly TextInputBaseField<string> LabelInputField;

        public StateNode(string nodeName, Vector2 position) : base(nodeName)
        {
            LabelInputField = Header.WithInputLabel(nodeName);
            LabelInputField.style.display = DisplayStyle.None;
            ApplyBaseStyle();
            style.left = position.x;
            style.top = position.y;

            Transitions = new List<StateTransition>();
            RegisterCallback<PointerDownEvent>(async e =>
            {
                if (e.button == 1)
                {
                    StateTransition transition = await Fabric.Transitions.RouteTransitionAsync(this, RequestTransition, CheckValid);
                    if (transition != null) Transitions.Add(transition);
                    bool CheckValid(StateNode stateNode) => Transitions.All(item => item.Target != stateNode);
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

        public override void Repaint()
        {
            base.Repaint();
            foreach (StateTransition transition in Transitions) transition.SendToBack();
        }

        public void RemoveTransitionAt(int index)
        {
            StateTransition transition = Transitions[index];
            Transitions.RemoveAt(index);
            Fabric.Transitions.DestroyTransition(this, transition);
        }
    }
}