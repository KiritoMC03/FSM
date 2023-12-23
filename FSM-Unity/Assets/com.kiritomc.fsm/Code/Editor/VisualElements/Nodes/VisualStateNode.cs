using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualStateNode : VisualNode, IVisualNodeWithTransitions
    {
        private readonly StatesContext context;
        public readonly TextInputBaseField<string> LabelInputField;

        public List<VisualStateTransition> Transitions { get; set; } = new List<VisualStateTransition>();

        public VisualStateNode(string stateName, StatesContext context, Vector2 position = default) : base(stateName)
        {
            this.context = context;
            LabelInputField = Header.NodeInputLabel(stateName);
            LabelInputField.style.display = DisplayStyle.None;
            style.left = position.x;
            style.top = position.y;
            this.AddManipulator(new StateNodeLabelManipulator(this, ValidatePotentialName));
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteTransitionManipulator<VisualStateNode>(this, context));
        }

        public string ValidatePotentialName(ChangeEvent<string> changeEvent)
        {
            string newName = changeEvent.newValue;
            int num = 1;
            if (!context.Nodes.Exists(i => i.Name == newName)) return newName;
            while (context.Nodes.Exists(i => i.Name == $"{newName} {num}")) num++;
            return $"{newName} {num}";
        }

        public void RemoveTransitionAt(int index)
        {
            VisualStateTransition transition = Transitions[index];
            Transitions.RemoveAt(index);
            Remove(transition);
            transition.Dispose();
        }
    }
}