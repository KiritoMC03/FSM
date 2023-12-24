using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Serialization;

namespace FSM.Runtime
{
    [Serializable]
    public class NotCondition : IFunction<bool>
    {
        public ParamNode<bool> Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute()
        {
            return !Value.Execute();
        }
    }

    [Serializable]
    public class OrCondition : IFunction<bool>
    {
        public ParamNode<bool> Left;
        public ParamNode<bool> Right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute()
        {
            return Left.Execute() || Right.Execute();
        }
    }

    [Serializable]
    public class AndCondition : IFunction<bool>
    {
        public ParamNode<bool> Left;
        public ParamNode<bool> Right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute()
        {
            return Left.Execute() && Right.Execute();
        }
    }
}