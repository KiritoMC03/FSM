using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Serialization;

namespace FSM.Runtime
{
    [Serializable]
    public class Not : IFunction<bool>
    {
        public ParamNode<bool> Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Execute()
        {
            return !Value.Execute();
        }
    }

    [Serializable]
    public class Or : IFunction<bool>
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
    public class And : IFunction<bool>
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