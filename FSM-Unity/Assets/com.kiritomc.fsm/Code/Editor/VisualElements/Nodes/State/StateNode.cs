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
        public List<VisualStateTransition> Transitions;
        public readonly TextInputBaseField<string> LabelInputField;

        public StateNode(string nodeName, Vector2 position) : base(nodeName)
        {
            LabelInputField = Header.WithInputLabel(nodeName);
            LabelInputField.style.display = DisplayStyle.None;
            ApplyBaseStyle();
            style.left = position.x;
            style.top = position.y;

            Transitions = new List<VisualStateTransition>();
            RegisterCallback<PointerDownEvent>(async e =>
            {
                if (e.button == 1)
                {
                    VisualStateTransition transition = await Fabric.Transitions.RouteTransitionAsync(this, RequestTransition, CheckValid);
                    if (transition != null) Transitions.Add(transition);
                    bool CheckValid(StateNode stateNode) => Transitions.All(item => item.Target != stateNode);
                }
            });
        }

        public override void Repaint()
        {
            base.Repaint();
            foreach (VisualStateTransition transition in Transitions) transition.SendToBack();
        }
    }
}