namespace FSM.Runtime
{
    public interface ITransition
    {
        IState To { get; }
        IConditionalLayoutNode Condition { get; }
    }
}