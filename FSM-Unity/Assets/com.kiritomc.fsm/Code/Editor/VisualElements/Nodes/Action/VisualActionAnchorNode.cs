using System;
using System.Linq;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualActionAnchorNode : VisualNode
    {
        private const string OnEnter = "OnEnter";
        private const string OnUpdate = "OnUpdate";
        private const string OnExit = "OnExit";

        private readonly StateContext context;
        private VisualNodeFieldLinksRegistration visualNodeFieldLinksRegistration;
        public IVisualNodeWithLinkExit OnEnterLink;
        public IVisualNodeWithLinkExit OnUpdateLink;
        public IVisualNodeWithLinkExit OnExitLink;

        public VisualActionAnchorNode(StateContext context, Vector2 position = default) : base("Anchor")
        {
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteVisualNodeLinkManipulator(context));
            style.left = position.x;
            style.top = position.y;
            Create(OnEnter);
            Create(OnUpdate);
            Create(OnExit);

            void Create(string fieldName) => new VisualNodeLinkRegistration(this, fieldName, NodeLinkRequest.NewAsync(this, Check), HandleLinked, GetCurrentLinkedNode, Check);
            bool Check(IVisualNodeWithLinkExit target) => target is VisualActionNode;
        }

        public void ForceLinkTo(string fieldName, IVisualNodeWithLinkExit target)
        {
            visualNodeFieldLinksRegistration.Refresh(fieldName, target);
        }

        public virtual void HandleLinked(string fieldName, IVisualNodeWithLinkExit target)
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

        protected virtual IVisualNodeWithLinkExit GetCurrentLinkedNode(string fieldName)
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