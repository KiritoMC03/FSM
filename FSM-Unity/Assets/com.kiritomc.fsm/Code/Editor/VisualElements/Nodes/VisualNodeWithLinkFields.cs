using System;
using System.Collections.Generic;

namespace FSM.Editor
{
    public class VisualNodeWithLinkFields : VisualNodeWithLinkExit
    {
        public readonly Dictionary<string, IVisualNodeWithLinkExit> Linked = new Dictionary<string, IVisualNodeWithLinkExit>();
        protected VisualNodeFieldLinksRegistration visualNodeFieldLinksRegistration;

        public VisualNodeWithLinkFields(Type type) : base(type.Name)
        {
            visualNodeFieldLinksRegistration = new VisualNodeFieldLinksRegistration(this, type, NodeLinkRequest.NewAsync(this), HandleLinked, GetCurrentLinkedNode);
        }

        public void ForceLinkTo(string fieldName, IVisualNodeWithLinkExit target)
        {
            visualNodeFieldLinksRegistration.Refresh(fieldName, target);
        }

        public virtual void HandleLinked(string fieldName, IVisualNodeWithLinkExit target)
        {
            Linked[fieldName] = target;
        }

        protected virtual IVisualNodeWithLinkExit GetCurrentLinkedNode(string fieldName)
        {
            return Linked.GetValueOrDefault(fieldName);
        }
    }
}