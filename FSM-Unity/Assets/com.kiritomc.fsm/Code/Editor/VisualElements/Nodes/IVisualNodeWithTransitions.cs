using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public interface IVisualNodeWithTransitions : IEventHandler
    {
        List<VisualStateTransition> Transitions { get; }
    }

    public interface IVisualNodeWithLinkExit : IEventHandler
    {
        Vector2 GetAbsoluteLinkPointPos();
    }
}