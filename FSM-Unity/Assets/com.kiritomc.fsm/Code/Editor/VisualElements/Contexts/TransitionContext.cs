using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class TransitionContext : VisualNodesContext<VisualNodeWithLinkExit>
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
            this.AddManipulator(new CreateVisualNodeManipulator<VisualNodeWithLinkExit>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualNodeWithLinkExit>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualNodeWithLinkExit>(this));
        }

        private Dictionary<string, Action> GetAvailableNodes()
        {
            return NodeTypes.InTransitionContext().ToDictionary(t => t.Name, t =>
            {
                if (NodeTypes.Condition.IsAssignableFrom(t))
                    return () => ProcessNewNode(new VisualConditionNode(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));
                if (NodeTypes.FunctionBool.IsAssignableFrom(t))
                    return () => ProcessNewFuncNode(new VisualFunctionNode<bool>(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));

                Debug.LogError("");
                return default(Action);
            });
        }

        public void ProcessNewNode<T>(T node) where T: VisualConditionNode
        {
            Add(node);
            Nodes.Add(node);
        }

        public void ProcessNewFuncNode<T>(T node) where T: VisualFunctionNode<bool>
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

        public override void Remove(VisualNodeWithLinkExit node)
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