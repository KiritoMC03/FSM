using System;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class VisualNodeLinkRequestEvent : EventBase<VisualNodeLinkRequestEvent>
    {
        public Action<VisualNodeWithLinkExit> TargetGotCallback { get; set; }
        public Func<VisualNodeWithLinkExit, bool> TargetMatchCallback { get; set; }
    }
}