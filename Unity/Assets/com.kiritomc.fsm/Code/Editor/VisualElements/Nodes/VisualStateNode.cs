using System.Collections.Generic;
using System.Linq;
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
        private readonly VisualElement fieldsContainer;

        public List<VisualStateTransitionData> Transitions { get; set; } = new List<VisualStateTransitionData>();

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
            fieldsContainer = new VisualElement()
            {
                style =
                {
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                },
            };
            Add(fieldsContainer);
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

        public VisualStateTransitionData AddTransition(VisualStateNode targetNode, Vector2 anchorNodePosition = default)
        {
            VisualStateTransition transition = new VisualStateTransition(this, targetNode, anchorNodePosition);
            VisualNodeTransitionField field = new VisualNodeTransitionField(targetNode.Name, transition);
            fieldsContainer.Add(field);
            VisualStateTransitionData data = new VisualStateTransitionData(transition, field);
            data.OnFieldPriorityUpClicked = field.SubscribePriorityUpClicked(UpPriority);
            data.OnFieldPriorityUpClicked = field.SubscribePriorityDownClicked(DownPriority);
            Transitions.Add(data);
            transition.Repaint();
            return data;

            void UpPriority()
            {
                int indexOf = Transitions.IndexOf(data);
                if (indexOf == 0) return;
                Transitions.RemoveAt(indexOf);
                Transitions.Insert(indexOf - 1, data);
                SortTransitions();
            }

            void DownPriority()
            {
                int indexOf = Transitions.IndexOf(data);
                if (indexOf == Transitions.Count - 1) return;
                Transitions.RemoveAt(indexOf);
                Transitions.Insert(indexOf + 1, data);
                SortTransitions();
            }
        }

        public void RemoveTransitionAt(int index)
        {
            VisualStateTransitionData data = Transitions[index];
            Transitions.RemoveAt(index);
            fieldsContainer.Remove(data.Field);
            data.Dispose();
        }

        public override void Repaint()
        {
            base.Repaint();
            Transitions.Select(d => d.Transition).Repaint();
        }

        private void SortTransitions()
        {
            fieldsContainer.Sort((transitionA, transitionB) =>
            {
                int indexA = Transitions.FindIndex(element => element.Field == transitionA);
                int indexB = Transitions.FindIndex(element => element.Field == transitionB);
                return indexA - indexB;
            });
        }
    }
}