namespace FSM.Runtime
{
    public interface IFsmAgent
    {
        public IState CurrentState { get; internal set; }
    }
}