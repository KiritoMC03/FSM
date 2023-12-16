﻿using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : Context
    {
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public List<StateNode> StateNodes = new List<StateNode>();
        public readonly string Name;

        public StatesContext(string name)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateNodeManipulator<StateNode>(GetAvailableNodes));
        }

        public Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                {"Simple State", () =>
                    {
                        const string nodeName = "Simple state";
                        int num = 0;
                        while (StateNodes.Exists(i => i.Name == $"{nodeName} {num}")) num++;
                        StateNode node = Fabric.Nodes.CreateStateNode($"{nodeName} {num}", this);
                        Add(node);
                        StateNodes.Add(node);
                    }
                },
            };
        }
    }
}