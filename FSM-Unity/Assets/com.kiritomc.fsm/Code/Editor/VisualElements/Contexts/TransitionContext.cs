﻿using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : NodesContext<ConditionalNode>
    {
        private readonly StateTransition target;

        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public TransitionContext(StateTransition target, string name)
        {
            this.target = target;
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions()
                .AddManipulator(new CreateNodeManipulator<ConditionalNode>(GetAvailableNodes));
        }

        private Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                {"Not", () => ProcessNewNode(Fabric.Nodes.ConditionalNotNode(this)) },
                {"Or", () => ProcessNewNode(Fabric.Nodes.ConditionalOrNode(this)) },
                {"And", () => ProcessNewNode(Fabric.Nodes.ConditionalAndNode(this)) },
            };
        }

        public void ProcessNewNode<T>(T node) where T: ConditionalNode
        {
            Add(node);
            Nodes.Add(node);
        }
    }
}