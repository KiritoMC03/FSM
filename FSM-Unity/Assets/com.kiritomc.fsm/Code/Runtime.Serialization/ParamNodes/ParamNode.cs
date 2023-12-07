using System;
using Newtonsoft.Json;
using UnityEngine;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class ParamNode<T>
    {
        [SerializeField] [JsonProperty]
        internal AbstractSerializableType<IFunction<T>> function;

        public ParamNode() { }

        public ParamNode(IFunction<T> function)
        {
            this.function = new AbstractSerializableType<IFunction<T>>(function);
        }

        public T Execute() => function.Item.Execute();
    }
}