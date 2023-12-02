using System.Collections.Generic;

namespace FSM.Runtime
{
    public interface IState
    {
        IEnumerable<ITransition> Transitions { get; }
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}