using System;
using System.Collections.Generic;

namespace FSM.Editor
{
    public static class PrettyTypes
    {
        public static readonly Dictionary<Type, string> Map = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(ushort), "ushort" },
        };

        public static string Get(Type type) => Map.TryGetValue(type, out string str) ? str : type.Name;
        public static string Pretty(this Type type) => Get(type);
    }
}