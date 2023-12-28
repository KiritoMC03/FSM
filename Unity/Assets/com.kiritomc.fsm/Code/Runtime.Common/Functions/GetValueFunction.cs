using System;
using Newtonsoft.Json;
using UnityEngine;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class GetValueFunction<T> : IFunction<T>
    {
        [SerializeField] [JsonProperty]
        private T value;

        public GetValueFunction(T value)
        {
            this.value = value;
        }
        
        public T Execute()
        {
            return value;
        }
    }
}