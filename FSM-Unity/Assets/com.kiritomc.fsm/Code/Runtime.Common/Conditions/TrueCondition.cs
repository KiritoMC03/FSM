using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Serialization;

namespace FSM.Runtime
{
    [Serializable]
    public class TrueCondition : ICondition
    {
        public ParamNode<bool> Special;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute() => true;
    }
}