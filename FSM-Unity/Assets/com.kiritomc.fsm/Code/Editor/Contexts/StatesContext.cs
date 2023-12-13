using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : Context
    {
        public List<StateNode> StateNodes = new List<StateNode>();
        private readonly EditorState editorState;
        private readonly Fabric fabric;

        public StatesContext(EditorState editorState, Fabric fabric)
        {
            this.editorState = editorState;
            this.fabric = fabric;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateNodeManipulator<StateNode>(fabric, GetAvailableNodes));
        }

        public Dictionary<string, Func<StateNode>> GetAvailableNodes()
        {
            return new Dictionary<string, Func<StateNode>>
            {
                {"Simple State", () =>
                    {
                        const string nodeName = "Simple state";
                        int num = 0;
                        while (StateNodes.Exists(i => i.StateName == $"{nodeName} {num}")) num++;
                        StateNode node = fabric.CreateStateNode($"{nodeName} {num}", this);
                        Add(node);
                        StateNodes.Add(node);
                        return node;
                    }
                },
            };
        }
    }
}