namespace FSM.Runtime
{
    public interface ITransition
    {
        IState From { get; }
        IState To { get; }
    }
}