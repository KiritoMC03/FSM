namespace FSM.Runtime
{
    public class FalseCondition : ICondition
    {
        public bool Decide() => false;
    }
}