using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : VisualNodesContext<VisualStateNode>
    {

        public StatesContext(string name)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateVisualNodeManipulator<VisualStateNode>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualStateNode>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualStateNode>(this));
        }

        public Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                {"Simple State", () =>
                    {
                        const string nodeName = "Simple state";
                        int num = 0;
                        while (Nodes.Exists(i => i.Name == $"{nodeName} {num}")) num++;
                        VisualStateNode node = new VisualStateNode($"{nodeName} {num}", this, this.WorldToLocal(EditorState.PointerPosition.Value));
                        ProcessNewNode(node);
                    }
                },
            };
        }

        public override void Remove(VisualStateNode node)
        {
            base.Remove(node);
            foreach (VisualStateNode other in Nodes)
            {
                for (int i = other.Transitions.Count - 1; i >= 0; i--)
                {
                    VisualStateTransition transition = other.Transitions[i];
                    if (transition.Target == node || transition.Source == node) other.RemoveTransitionAt(i);
                }
            }
        }
    }
}