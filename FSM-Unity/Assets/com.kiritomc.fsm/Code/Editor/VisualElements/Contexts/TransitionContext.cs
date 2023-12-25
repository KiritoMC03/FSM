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
                if (t.IsICondition())
                    return () => ProcessNewNode(new VisualConditionNode(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));
                if (t.IsIFunctionBool())
                    return () => ProcessNewNode(new VisualFunctionNode<bool>(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));

                Debug.LogError("");
                return default(Action);
            });
        }

        public override void Remove(VisualNodeWithLinkExit node)
        {
            base.Remove(node);
            foreach (VisualNodeWithLinkExit other in Nodes)
            {
                if (other is VisualNodeWithLinkFields nodeWithLinkFields)
                {
                    foreach ((string fieldName, IVisualNodeWithLinkExit linked) in nodeWithLinkFields.Linked)
                    {
                        if (linked == node) nodeWithLinkFields.ForceLinkTo(fieldName, default);
                    }
                }
            }
        }
    }
}