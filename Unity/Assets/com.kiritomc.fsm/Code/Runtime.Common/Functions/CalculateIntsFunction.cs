using System;
using FSM.Runtime.Serialization;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class CalculateIntsFunction : IFunction<int>
    {
        public ParamNode<int> A;
        public ParamNode<int> B;
        public ParamNode<Operator> Operator;

        public int Execute()
        {
            return Operator.Execute() switch
            {
                Common.Operator.Add => A.Execute() + B.Execute(),
                Common.Operator.Subtract => A.Execute() - B.Execute(),
                Common.Operator.Multiply => A.Execute() * B.Execute(),
                Common.Operator.Divide => A.Execute() / B.Execute(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}