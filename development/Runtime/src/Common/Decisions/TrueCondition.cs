namespace FSM.Runtime
{
    public class TrueCondition : ICondition
    {
        public bool Decide() => true;
    }
}