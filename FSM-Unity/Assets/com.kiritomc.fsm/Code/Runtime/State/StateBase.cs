using System.Collections.Generic;

namespace FSM.Runtime
{
    public class StateBase : IState
    {
        public readonly string Name;
        private readonly ActionsExecutor actionsExecutor = new ActionsExecutor();
        // private readonly ActionLayoutNode actions;
        private IEnumerable<ITransition> outgoingTransitions;

        public StateBase(string name, IEnumerable<ITransition> outgoingTransitions)
        {
            this.Name = name;
            this.outgoingTransitions = outgoingTransitions;
        }

        public void SetTransitions(IEnumerable<ITransition> outgoingTransitions)
        {
            this.outgoingTransitions = outgoingTransitions;
        }

        #region IState

        IEnumerable<ITransition> IState.OutgoingTransitions => outgoingTransitions;

        void IState.OnEnter()
        {
            // ToDo:
        }

        void IState.OnUpdate()
        {
            // ToDo: actionsExecutor.Execute(actions);
        }

        void IState.OnExit()
        {
            // ToDo:
        }

        #endregion
    }
}