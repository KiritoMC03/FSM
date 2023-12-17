using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using FSM.Runtime;
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
                .DefaultInteractions();
            this.AddManipulator(new CreateNodeManipulator<ConditionalNode>(GetAvailableNodes));
            this.AddManipulator(new SelectNodesManipulator<ConditionalNode>(this));
            this.AddManipulator(new DeleteStateNodeManipulator<ConditionalNode>(this));
        }

        private Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                {"Not", () => ProcessNewNode(Fabric.Nodes.ConditionalNotNode(this)) },
                {"Or", () => ProcessNewNode(Fabric.Nodes.ConditionalOrNode(this)) },
                {"And", () => ProcessNewNode(Fabric.Nodes.ConditionalAndNode(this)) },
                { "True Condition", () => ProcessNewNode(Fabric.Nodes.ConditionalConditionNode(this, new ConditionLayoutNode(new TrueCondition()))) },
            };
        }

        public void ProcessNewNode<T>(T node) where T: ConditionalNode
        {
            Add(node);
            Nodes.Add(node);
        }

        public override void Remove(ConditionalNode node)
        {
            base.Remove(node);
            // foreach (ConditionalNode other in Nodes)
            // {
            //     for (int i = other.conn.Transitions.Count - 1; i >= 0; i--)
            //     {
            //         StateTransition transition = other.Transitions[i];
            //         if (transition.Target == node || transition.Source == node) other.RemoveTransitionAt(i);
            //     }
            // }
        }
    }
}