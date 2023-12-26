using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualTransitionAnchorNode : VisualNode
    {
        public const string ConditionLabel = "Condition";
        private readonly TransitionContext context;
        public readonly VisualNodeLinkRegistration ConditionLinkRegistration;
        public VisualNodeWithLinkExit ConditionLink;

        public VisualTransitionAnchorNode(TransitionContext context, Vector2 position = default) : base("Anchor", -1)
        {
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteVisualNodeLinkManipulator(context));
            style.left = position.x;
            style.top = position.y;
            ConditionLinkRegistration = Create(ConditionLabel);

            VisualNodeLinkRegistration Create(string fieldName) => new VisualNodeLinkRegistration(this, fieldName, fieldName, NodeLinkRequest.NewAsync(this, Check), HandleLinkedConditionNode, GetCurrentLinkedConditionNode, Check);
            bool Check(VisualNodeWithLinkExit target) => target is VisualConditionNode or VisualFunctionNode<bool>;
        }

        public void ForceLinkCondition(VisualNodeWithLinkExit target)
        {
            ConditionLinkRegistration.SetTarget(target);
        }

        public virtual void HandleLinkedConditionNode(string fieldName, VisualNodeWithLinkExit target)
        {
            ConditionLink = target;
        }

        protected virtual VisualNodeWithLinkExit GetCurrentLinkedConditionNode(string fieldName)
        {
            return ConditionLink;
        }
    }
}