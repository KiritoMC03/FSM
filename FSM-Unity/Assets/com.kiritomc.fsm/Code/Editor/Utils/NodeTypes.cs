using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Runtime;
using UnityEngine;

namespace FSM.Editor
{
    public static class NodeTypes
    {
        public static readonly Type Function = typeof(IFunction<>);
        public static readonly Type FunctionBool = typeof(IFunction<bool>);

        public static readonly Type Condition = typeof(ICondition);

        public static readonly Type CSharpObject = typeof(System.Object);
        
        public static IReadOnlyList<Type> InTransitionContext()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(Filter)
                .ToArray();

            bool Filter(Type type)
            {
                if (type == typeof(TrueCondition))
                {
                    Debug.Log($"Found");
                }
                return !type.IsInterface
                       && type != CSharpObject
                       && (FunctionBool.IsAssignableFrom(type) || Condition.IsAssignableFrom(type));
            }
        }
    }
}