using FSM.Runtime;

namespace FSM.Runtime
{
    public class FalseDecision : IDecision
    {
        public bool Decide() => false;
    }
}