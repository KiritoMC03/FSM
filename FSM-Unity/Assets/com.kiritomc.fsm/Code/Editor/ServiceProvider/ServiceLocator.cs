using System;
using System.Collections.Generic;

namespace FSM.Editor
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> items = new Dictionary<Type, object>();

        public static ServiceLocator Instance { get; } = new ServiceLocator();

        private ServiceLocator() { }

        public T Get<T>() => (T)items[typeof(T)];
        public void Set<T>(T item) => items[typeof(T)] = item;
    }
}