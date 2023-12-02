using System;
using System.Collections.Generic;

namespace FSM.Runtime.Utils
{
    public class GenericPool<T> where T: IPoolable
    {
        private Stack<T> items;
        private FactoryMethod<T> factoryMethod;

        public GenericPool(FactoryMethod<T> factoryMethod, int initialCapacity = 2)
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

            T item = items.Pop();
            return item;
        }

        public void Push(T item)
        {
            item.Reset();
            items.Push(item);
        }
    }
}