using System;
using System.Linq;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualActionNode : VisualNodeWithLinkFields
    {
        public readonly Type ActionType;
        public IVisualNodeWithLinkExit DependentAction;
        public VisualNodeLinkRegistration DependentActionLinkRegistration;
        private readonly StateContext context;
        private const string DependentActionLabel = "Dependent Action";

        public VisualActionNode(Type actionType, StateContext context, Vector2 position = default) : base(actionType)
        {
            DependentActionLinkRegistration = new VisualNodeLinkRegistration(this, DependentActionLabel, NodeLinkRequest.NewAsync(this), HandleActionLinked, GetCurrentLinkedActionNode);
            Remove(DependentActionLinkRegistration.LinkFieldView);
            Insert(IndexOf(visualNodeFieldLinksRegistration.Items.First().Value.LinkFieldView), DependentActionLinkRegistration.LinkFieldView);
            this.ActionType = actionType;
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteConnectionManipulator(context));
            style.left = position.x;
            style.top = position.y;
        }

        public void ForceLinkAction(IVisualNodeWithLinkExit target)
        {
            DependentAction = target;
            DependentActionLinkRegistration.SetTarget(target);
        }

        private void HandleActionLinked(string _, IVisualNodeWithLinkExit target) => DependentAction = target;
        private  IVisualNodeWithLinkExit GetCurrentLinkedActionNode(string _) => DependentAction;
    }
}