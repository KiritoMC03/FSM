using System.Collections.Generic;

namespace FSM.Runtime
{
    public class StateBase : IState
    {
        private readonly ActionsExecutor actionsExecutor = new ActionsExecutor();
        private readonly ActionLayoutNode actions;
        private IEnumerable<ITransition> outgoingTransitions;

        public StateBase(ActionLayoutNode actions, IEnumerable<ITransition> outgoingTransitions)
        {
            this.actions = actions;
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
            actionsExecutor.Execute(actions);
        }

        void IState.OnExit()
        {
            // ToDo:
        }

        #endregion
    }
}