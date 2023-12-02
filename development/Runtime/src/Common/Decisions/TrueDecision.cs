using FSM.Runtime;

namespace FSM.Runtime
{
    public class TrueDecision : IDecision
    {
        public bool Decide() => true;
    }
}