using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Runtime;
using UnityEngine;

namespace FSM.Editor
{
    public static class NodeTypesExtensions
    {
        public static bool EqualsCSharpObject(this Type type) => type == typeof(System.Object);
        public static bool IsIAction(this Type type) => typeof(IAction).IsAssignableFrom(type);
        public static bool IsICondition(this Type type) => typeof(ICondition).IsAssignableFrom(type);
        public static bool IsIFunctionBool(this Type type) => typeof(IFunction<bool>).IsAssignableFrom(type);
        public static bool IsIFunction(this Type type)
        {
            Type targetInterface = typeof(IFunction<>);
            return type.GetInterfaces().Any(i => 
                i.IsGenericType && i.GetGenericTypeDefinition() == targetInterface);
        }

        public static bool IsNot(this Type type) => typeof(Not).IsAssignableFrom(type);
        public static bool IsOr(this Type type) => typeof(Or).IsAssignableFrom(type);
        public static bool IsAnd(this Type type) => typeof(And).IsAssignableFrom(type);
    }

    public static class NodeTypes
    {
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
                return !type.IsInterface && !type.EqualsCSharpObject() && (type.IsICondition() || type.IsIFunctionBool());
            }
        }

        public static IReadOnlyList<Type> InStateContext()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(Filter)
                .ToArray();

            bool Filter(Type type)
            {
                return !type.IsInterface && !type.EqualsCSharpObject() && (type.IsIAction() || type.IsIFunction());
            }
        }
    }
}