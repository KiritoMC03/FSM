using System.Collections.Generic;
using FSM.Editor.Extensions;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualStateNode : VisualNode
    {
        public readonly StatesContext ParentContext;
        public readonly StateContext Context;
        public readonly TextInputBaseField<string> LabelInputField;

        public List<VisualStateTransition> Transitions { get; set; } = new List<VisualStateTransition>();

        public VisualStateNode(string stateName, StatesContext parentContext, Vector2 position = default, Vector2 anchorNodePosition = default) : 
            this(stateName, parentContext.GetFreeId(), parentContext, position, anchorNodePosition)
        {
        }

        public VisualStateNode(string stateName, int id, StatesContext parentContext, Vector2 position = default, Vector2 anchorNodePosition = default) : base(stateName, id)
        {
            parentContext.ReserveId(id, this);
            ParentContext = parentContext;
            Context = new StateContext(stateName, anchorNodePosition);
            LabelInputField = Header.NodeInputLabel(stateName);
            LabelInputField.style.display = DisplayStyle.None;
            style.left = position.x;
            style.top = position.y;
            this.AddManipulator(new OpenStateContextManipulator(this));
            this.AddManipulator(new StateNodeLabelManipulator(this, ValidatePotentialName));
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteTransitionManipulator<VisualStateNode>(this, parentContext));
            Repaint();
        }

        public string ValidatePotentialName(ChangeEvent<string> changeEvent)
        {
            string newName = changeEvent.newValue;
            int num = 1;
            if (!ParentContext.Nodes.Exists(i => i.Name == newName)) return newName;
            while (ParentContext.Nodes.Exists(i => i.Name == $"{newName} {num}")) num++;
            return $"{newName} {num}";
        }

        public VisualStateTransition AddTransition(VisualStateNode targetNode)
        {
            VisualStateTransition transition = new VisualStateTransition(this, targetNode);
            Add(transition);
            Transitions.Add(transition);
            transition.Repaint();
            return transition;
        }

        public void RemoveTransitionAt(int index)
        {
            VisualStateTransition transition = Transitions[index];
            Transitions.RemoveAt(index);
            Remove(transition);
            transition.Dispose();
        }

        public override void Repaint()
        {
            base.Repaint();
            Transitions.Repaint();
        }
    }
}