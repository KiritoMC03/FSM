namespace FSM.Runtime
{
    public class FsmAgentBase : IFsmAgent
    {
        public IState CurrentState { get; set; }

        public FsmAgentBase(IState initialState)
        {
            CurrentState = initialState;
        }
    }
}