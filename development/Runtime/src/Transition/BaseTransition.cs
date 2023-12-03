namespace FSM.Runtime
{
    public class BaseTransition : ITransition
    {
        public IState To { get; }
        public ILayoutNode Condition { get; }

        public BaseTransition(IState to, ILayoutNode condition)
        {
            To = to;
            Condition = condition;
        }
    }
}