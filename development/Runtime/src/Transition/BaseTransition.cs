namespace FSM.Runtime
{
    public class BaseTransition : ITransition
    {
        public IState To { get; }
        public IConditionalLayoutNode Condition { get; }

        public BaseTransition(IState to, IConditionalLayoutNode condition)
        {
            To = to;
            Condition = condition;
        }
    }
}