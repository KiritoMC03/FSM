using System;
using System.Runtime.CompilerServices;

namespace FSM.Runtime
{
    [Serializable]
    public class FalseCondition : ICondition
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute() => false;
    }
}