using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualActionAnchorNode : VisualNode
    {
        public const string OnEnter = "OnEnter";
        public const string OnUpdate = "OnUpdate";
        public const string OnExit = "OnExit";

        private readonly StateContext context;
        public readonly VisualNodeLinkRegistration OnEnterLinkRegistration;
        public readonly VisualNodeLinkRegistration OnUpdateLinkRegistration;
        public readonly VisualNodeLinkRegistration OnExitLinkRegistration;
        public VisualNodeWithLinkExit OnEnterLink;
        public VisualNodeWithLinkExit OnUpdateLink;
        public VisualNodeWithLinkExit OnExitLink;

        public VisualActionAnchorNode(StateContext context, Vector2 position = default) : base("Anchor", -1)
        {
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteVisualNodeLinkManipulator(context));
            style.left = position.x;
            style.top = position.y;
            OnEnterLinkRegistration = Create(OnEnter);
            OnUpdateLinkRegistration = Create(OnUpdate);
            OnExitLinkRegistration = Create(OnExit);

            VisualNodeLinkRegistration Create(string fieldName) => new VisualNodeLinkRegistration(this, fieldName, fieldName, NodeLinkRequest.NewAsync(this, Check), HandleLinked, GetCurrentLinkedNode, Check);
            bool Check(VisualNodeWithLinkExit target) => target is VisualActionNode;
        }

        public void ForceLinkTo(string fieldName, VisualNodeWithLinkExit target)
        {
            switch (fieldName)
            {
                case OnEnter:
                    OnEnterLinkRegistration.SetTarget(target);
                    break;
                case OnUpdate:
                    OnUpdateLinkRegistration.SetTarget(target);
                    break;
                case OnExit:
                    OnExitLinkRegistration.SetTarget(target);
                    break;
            }
        }

        public virtual void HandleLinked(string fieldName, VisualNodeWithLinkExit target)
        {
            switch (fieldName)
            {
                case OnEnter:
                    OnEnterLink = target;
                    break;
                case OnUpdate:
                    OnUpdateLink = target;
                    break;
                case OnExit:
                    OnExitLink = target;
                    break;
            }
        }

        protected virtual VisualNodeWithLinkExit GetCurrentLinkedNode(string fieldName)
        {
            return fieldName switch
            {
                OnEnter => OnEnterLink,
                OnUpdate => OnUpdateLink,
                OnExit => OnExitLink,
                _ => default,
            };
        }
    }
}