using System;
using System.Collections.Generic;
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

        public Dictionary<string, Func<ConditionalNode>> GetAvailableNodes()
        {
            return new Dictionary<string, Func<ConditionalNode>>
            {
                {"Or", () =>
                    {
                        OrNode node = Fabric.TestConditional(this);
                        Add(node);
                        nodes.Add(node);
                        return node;
                    }
                },
            };
        }
    }
}