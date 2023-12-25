using System;
using System.Linq;
using FSM.Runtime;

namespace FSM.Editor
{
    public static class VisualNodesChecking
    {
        public static bool IsVisualFunctionNodeWithReturnType(this object target, Type returnType)
        {
            if (target is VisualFunctionNode visualFunctionNode)
            {
                return visualFunctionNode.FunctionType.GetInterface(typeof(IFunction<>).Name).GetGenericArguments().First() == returnType;
            }
            return false;
        }
    }
}