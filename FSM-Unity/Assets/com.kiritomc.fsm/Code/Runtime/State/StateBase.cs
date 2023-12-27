using System.Collections.Generic;

namespace FSM.Runtime
{
    public class StateBase : IState
    {
        public readonly string Name;
        private readonly ActionsExecutor actionsExecutor = new ActionsExecutor();

        public ActionLayoutNode OnEnterActions { get; }
        public ActionLayoutNode OnUpdateActions { get; }
        public ActionLayoutNode OnExitActions { get; }

        public StateBase(string name, IEnumerable<ITransition> outgoingTransitions)
        {
            this.Name = name;
            this.OutgoingTransitions = outgoingTransitions;
        }

        #region IState

        public IEnumerable<ITransition> OutgoingTransitions { get; set; }

        void IState.OnEnter()
        {
            actionsExecutor.Execute(OnEnterActions);
        }

        void IState.OnUpdate()
        {
            actionsExecutor.Execute(OnUpdateActions);
        }

        void IState.OnExit()
        {
            actionsExecutor.Execute(OnExitActions);
        }

        #endregion
    }
}