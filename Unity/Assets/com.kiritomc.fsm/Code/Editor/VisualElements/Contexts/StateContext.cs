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
        public readonly VisualActionAnchorNode AnchorNode;

        public StateContext(string name, Vector2 anchorNodePosition = default)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            this.AddManipulator(new ContextDraggingManipulator<VisualNodeWithLinkExit>(this));
            this.AddManipulator(new ScaleContextManipulator<VisualNodeWithLinkExit>(this));
            this.AddManipulator(new CreateVisualNodeManipulator<VisualNodeWithLinkExit>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualNodeWithLinkExit>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualNodeWithLinkExit>(this));
            AnchorNode = new VisualActionAnchorNode(this)
            {
                style =
                {
                    left = anchorNodePosition.x,
                    top = anchorNodePosition.y,
                },
            };
            BuildContentContainer().Add(AnchorNode);
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
            if (AnchorNode.OnEnterLink == node) AnchorNode.OnEnterLinkRegistration.SetTarget(default);
            if (AnchorNode.OnUpdateLink == node) AnchorNode.OnUpdateLinkRegistration.SetTarget(default);
            if (AnchorNode.OnExitLink == node) AnchorNode.OnExitLinkRegistration.SetTarget(default);
            foreach (VisualNodeWithLinkExit other in Nodes)
            {
                if (other is VisualNodeWithLinkFields nodeWithLinkFields)
                {
                    if (nodeWithLinkFields is VisualActionNode actionNode && actionNode.DependentAction == node)
                    {
                        actionNode.ForceLinkAction(default);
                    }
                    foreach ((string fieldName, VisualNodeWithLinkExit linked) in nodeWithLinkFields.Linked)
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

        public override void MoveNodes(Vector2 offset)
        {
            base.MoveNodes(offset);
            AnchorNode.Placement += offset;
        }
    }
}