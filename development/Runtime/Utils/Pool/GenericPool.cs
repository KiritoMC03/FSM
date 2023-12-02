using System;
using System.Collections.Generic;

namespace FSM.Runtime.Utils
{
    public class GenericPool<T> where T: IPoolable
    {
        private Stack<T> items;
        private FactoryMethod<T> factoryMethod;

        public GenericPool(FactoryMethod<T> factoryMethod, int initialCapacity = 32)
        {
            this.factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
            this.items = new Stack<T>(initialCapacity);
            for (int i = 0; i < initialCapacity; i++) items.Push(factoryMethod());
        }

        public T Pop()
        {
            if (items.Count <= 0)
            {
                return factoryMethod();
            }

            var item = items.Pop();
            item.Reset();
            return item;
        }

        public void Push(T item) => items.Push(item);
    }
}