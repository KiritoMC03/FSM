using System;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class TransitionRequestEvent : EventBase<TransitionRequestEvent>
    {
        public Action<StateNode> SetTransitionCallback { get; set; }
    }
}