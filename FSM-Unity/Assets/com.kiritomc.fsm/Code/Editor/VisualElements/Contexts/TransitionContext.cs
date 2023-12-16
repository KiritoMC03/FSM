using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : Context
    {
        private readonly StateTransition target;
        private readonly List<ConditionalNode> nodes = new List<ConditionalNode>();
        public readonly string Name;
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
                {"Not", () => ProcessNewNode(Fabric.ConditionalNotNode(this)) },
                {"Or", () => ProcessNewNode(Fabric.ConditionalOrNode(this)) },
                {"And", () => ProcessNewNode(Fabric.ConditionalAndNode(this)) },
            };
        }

        private void ProcessNewNode<T>(T node) where T: ConditionalNode
        {
            Add(node);
            nodes.Add(node);
        }
    }
}