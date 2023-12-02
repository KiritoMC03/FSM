namespace FSM.Runtime
{
    public interface ITransition
    {
        IState To { get; }
        ILogicLayoutNode Condition { get; }
    }
}