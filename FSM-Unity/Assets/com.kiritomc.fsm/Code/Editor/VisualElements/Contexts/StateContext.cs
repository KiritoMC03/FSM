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
            if (anchorNode.OnEnterLink == node) anchorNode.OnEnterLinkRegistration.SetTarget(default);
            if (anchorNode.OnUpdateLink == node) anchorNode.OnUpdateLinkRegistration.SetTarget(default);
            if (anchorNode.OnExitLink == node) anchorNode.OnExitLinkRegistration.SetTarget(default);
            foreach (VisualNodeWithLinkExit other in Nodes)
            {
                if (other is VisualNodeWithLinkFields nodeWithLinkFields)
                {
                    if (nodeWithLinkFields is VisualActionNode actionNode && actionNode.DependentAction == node)
                    {
                        actionNode.ForceLinkAction(default);
                    }
                    foreach ((string fieldName, IVisualNodeWithLinkExit linked) in nodeWithLinkFields.Linked)
                    {
                        if (linked == node)
                        {
                            Debug.Log($"DFound");
                            nodeWithLinkFields.ForceLinkTo(fieldName, default);
                        }
                    }
                }
            }
        }
    }
}