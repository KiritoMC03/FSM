using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Serialization;

namespace FSM.Runtime
{
    [Serializable]
    public class TestCondition : ICondition
    {
        public ParamNode<int> SomeInput;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Decide()
        {
            return SomeInput.Execute() != 0;
        }
    }
}