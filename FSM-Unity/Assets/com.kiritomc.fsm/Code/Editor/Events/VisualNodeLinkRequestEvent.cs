using System;
using System.Numerics;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class VisualNodeLinkRequestEvent : EventBase<VisualNodeLinkRequestEvent>
    {
        public Action<IVisualNodeWithLinkExit> TargetGotCallback { get; set; }
        public Func<IVisualNodeWithLinkExit, bool> TargetMatchCallback { get; set; }
    }
}