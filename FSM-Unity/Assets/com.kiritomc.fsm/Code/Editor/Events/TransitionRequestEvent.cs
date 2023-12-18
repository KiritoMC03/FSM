using System;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class TransitionRequestEvent<TNode> : EventBase<TransitionRequestEvent<TNode>>
        where TNode: IVisualNodeWithTransitions
    {
        public Action<TNode> SetTransitionCallback { get; set; }
    }
}