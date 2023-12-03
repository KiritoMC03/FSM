namespace FSM.Runtime
{
    public interface ITransition
    {
        IState To { get; }
        ILayoutNode Condition { get; }
    }
}