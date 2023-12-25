using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StateContext : VisualNodesContext<VisualNodeWithLinkExit>
    {
        private readonly VisualActionAnchorNode anchorNode;

        public StateContext(string name)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new CreateVisualNodeManipulator<VisualNodeWithLinkExit>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualNodeWithLinkExit>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualNodeWithLinkExit>(this));
            anchorNode = new VisualActionAnchorNode(this);
            Add(anchorNode);
        }

        private Dictionary<string, Action> GetAvailableNodes()
        {
            return NodeTypes.InStateContext().ToDictionary(t => t.Name, t =>
            {
                if (t.IsIAction())
                    return () => ProcessNewNode(new VisualActionNode(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));
                if (t.IsIFunction())
                    return () => ProcessNewNode(new VisualFunctionNode(t, this, this.WorldToLocal(EditorState.PointerPosition.Value)));

                Debug.LogError("");
                return default(Action);
            });
        }

        public override void Remove(VisualNodeWithLinkExit node)
        {
            base.Remove(node);
            // foreach (VisualStateNode other in Nodes)
            // {
            //     for (int i = other.Transitions.Count - 1; i >= 0; i--)
            //     {
            //         VisualStateTransition transition = other.Transitions[i];
            //         if (transition.Target == node || transition.Source == node) other.RemoveTransitionAt(i);
            //     }
            // }
        }
    }
}