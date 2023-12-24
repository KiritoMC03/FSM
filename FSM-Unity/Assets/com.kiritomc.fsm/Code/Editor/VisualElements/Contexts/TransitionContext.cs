using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : VisualNodesContext<VisualConditionalNode>
    {
        private readonly VisualStateTransition target;

        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public TransitionContext(VisualStateTransition target, string name)
        {
            this.target = target;
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateVisualNodeManipulator<VisualConditionalNode>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualConditionalNode>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualConditionalNode>(this));
        }

        private Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                // {"Not", () => ProcessNewNode(Fabric.Nodes.ConditionalNotNode(this)) },
                // {"Or", () => ProcessNewNode(Fabric.Nodes.ConditionalOrNode(this)) },
                // {"And", () => ProcessNewNode(Fabric.Nodes.ConditionalAndNode(this)) },
                // { "True Condition", () => ProcessNewNode(Fabric.Nodes.ConditionalConditionNode(this, new ConditionLayoutNode(new TrueCondition()))) },
            };
        }

        public void ProcessNewNode<T>(T node) where T: VisualConditionalNode
        {
            Add(node);
            Nodes.Add(node);
        }

        public void Open()
        {
            EditorState.EditorRoot.Value.Remove(EditorState.CurrentContext.Value);
            EditorState.EditorRoot.Value.Add(this);
            EditorState.CurrentContext.Value = this;
        }

        public override void Remove(VisualConditionalNode node)
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