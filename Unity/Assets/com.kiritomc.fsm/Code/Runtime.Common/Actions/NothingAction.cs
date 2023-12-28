using System;
using System.Runtime.CompilerServices;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class NothingAction : IAction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute()
        {
        }
    }
}