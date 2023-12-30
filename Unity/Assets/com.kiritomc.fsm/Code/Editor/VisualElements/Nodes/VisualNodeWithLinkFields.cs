using System;
using System.Collections.Generic;

namespace FSM.Editor
{
    public class VisualNodeWithLinkFields : VisualNodeWithLinkExit
    {
        public readonly Dictionary<string, VisualNodeWithLinkExit> Linked = new Dictionary<string, VisualNodeWithLinkExit>();
        protected VisualNodeFieldLinksRegistration visualNodeFieldLinksRegistration;

        public VisualNodeWithLinkFields(Type type, int id, bool createFields = true) : base(type.Name, id)
        {
            if (createFields) CreateFields(type);
        }

        public void CreateFields(Type type)
        {
            visualNodeFieldLinksRegistration = new VisualNodeFieldLinksRegistration(this, type, HandleLinked, GetCurrentLinkedNode);
        }

        public void ForceLinkTo(string fieldName, VisualNodeWithLinkExit target)
        {
            visualNodeFieldLinksRegistration.Refresh(fieldName, target);
        }

        public virtual void HandleLinked(string fieldName, VisualNodeWithLinkExit target)
        {
            Linked[fieldName] = target;
        }

        protected virtual VisualNodeWithLinkExit GetCurrentLinkedNode(string fieldName)
        {
            return Linked.GetValueOrDefault(fieldName);
        }

        public override void Repaint()
        {
            base.Repaint();
            MarkDirtyRepaint();
        }
    }
}