using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : NodesContext<StateNode>
    {
        public List<StateNode> SelectedStateNodes = new List<StateNode>();

        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public StatesContext(string name)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateNodeManipulator<StateNode>(GetAvailableNodes));
            this.AddManipulator(new SelectNodesManipulator<StateNode>(this));
            this.AddManipulator(new DeleteStateNodeManipulator<StateNode>(this));
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
                        StateNode node = Fabric.Nodes.CreateStateNode($"{nodeName} {num}", this);
                        Add(node);
                        Nodes.Add(node);
                    }
                },
            };
        }

        public override void Remove(StateNode node)
        {
            base.Remove(node);
            foreach (StateNode other in Nodes)
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