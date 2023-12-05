using System;
using System.Runtime.CompilerServices;

namespace FSM.Runtime
{
    [Serializable]
    public class TrueCondition : ICondition
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Decide() => true;
    }
}