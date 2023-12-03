using System.Collections.Generic;

namespace FSM.Runtime
{
    public class StateBase : IState
    {
        private readonly ActionsExecutor actionsExecutor = new ActionsExecutor();
        private readonly ActionLayoutNode actions;

        public StateBase(ActionLayoutNode actions, IEnumerable<ITransition> outgoingTransitions)
        {
            this.actions = actions;
            OutgoingTransitions = outgoingTransitions;
        }

        internal void SetTransitions(IEnumerable<ITransition> outgoingTransitions)
        {
            OutgoingTransitions = outgoingTransitions;
        }

        #region IState

        public IEnumerable<ITransition> OutgoingTransitions { get; private set; }

        public void OnEnter()
        {
            // ToDo:
        }

        public void OnUpdate()
        {
            actionsExecutor.Execute(actions);
        }

        public void OnExit()
        {
            // ToDo:
        }

        #endregion
    }
}