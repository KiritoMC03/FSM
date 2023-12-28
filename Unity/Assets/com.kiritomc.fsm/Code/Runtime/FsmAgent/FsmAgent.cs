namespace FSM.Runtime
{
    public interface IFsmAgent
    {
        public IState CurrentState { get; set; }
    }
}