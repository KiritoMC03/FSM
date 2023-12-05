using System;

namespace FSM.Runtime
{
    [Serializable]
    public class TrueCondition : ICondition
    {
        public bool Decide() => true;
    }
}