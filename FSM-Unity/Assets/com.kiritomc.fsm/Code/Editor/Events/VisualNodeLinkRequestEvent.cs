using System;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class VisualNodeLinkRequestEvent : EventBase<VisualNodeLinkRequestEvent>
    {
        public Action<IVisualNodeWithLinkExit> TargetGotCallback { get; set; }
    }
}