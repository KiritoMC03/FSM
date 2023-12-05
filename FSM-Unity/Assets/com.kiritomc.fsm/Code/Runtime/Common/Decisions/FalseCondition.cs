using System;

namespace FSM.Runtime
{
    [Serializable]
    public class FalseCondition : ICondition
    {
        public bool Decide() => false;
    }
}