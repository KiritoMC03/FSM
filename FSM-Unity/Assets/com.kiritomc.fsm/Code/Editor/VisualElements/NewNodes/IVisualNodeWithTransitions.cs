using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public interface IVisualNodeWithTransitions : IEventHandler
    {
        List<VisualStateTransition> Transitions { get; }
    }

    public interface IVisualNodeWithConnection : IEventHandler
    {
        VisualNodeWithLinkExit
    }
}