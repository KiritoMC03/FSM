using System;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualActionNode : VisualNodeWithLinkFields
    {
        private const string DependentActionLabel = "Dependent Action";

        public readonly Type ActionType;
        public readonly VisualNodeLinkRegistration DependentActionLinkRegistration;
        private readonly StateContext context;

        public VisualActionNode DependentAction;

        public VisualActionNode(Type actionType, StateContext context, Vector2 position = default) : 
            this(actionType, context.GetFreeId(), context, position)
        {
        }

        public VisualActionNode(Type actionType, int id, StateContext context, Vector2 position = default) : base(actionType, id, false)
        {
            context.ReserveId(id, this);
            DependentActionLinkRegistration = new VisualNodeLinkRegistration(this, DependentActionLabel, DependentActionLabel, NodeLinkRequest.NewAsync(this, Check), HandleActionLinked, GetCurrentLinkedActionNode, Check);
            CreateFields(actionType);
            ActionType = actionType;
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteVisualNodeLinkManipulator(context));
            style.left = position.x;
            style.top = position.y;
            bool Check(VisualNodeWithLinkExit target) => target is VisualActionNode && target != this;
        }

        public void ForceLinkAction(VisualActionNode target)
        {
            DependentAction = target;
            DependentActionLinkRegistration.SetTarget(target);
        }

        private void HandleActionLinked(string _, VisualNodeWithLinkExit target) => DependentAction = (VisualActionNode)target;
        private  VisualNodeWithLinkExit GetCurrentLinkedActionNode(string _) => DependentAction;
    }
}