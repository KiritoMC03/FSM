using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : Context
    {
        private readonly List<StateNode> stateNodes = new List<StateNode>();
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
                        StateNode node = fabric.CreateStateNode("Simple State", this);
                        Add(node);
                        stateNodes.Add(node);
                        return node;
                    }
                },
            };
        }
    }
}