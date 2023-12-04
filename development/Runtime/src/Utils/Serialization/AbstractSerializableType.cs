using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Runtime.Utils.Serialization
{
    [Serializable]
    public class AbstractSerializableType<T>
    {
        [NonSerialized]
        public T Item;
        [JsonProperty]
        private Type type;
        [JsonProperty]
        private string serializedText;

        public AbstractSerializableType()
        {
        }

        public AbstractSerializableType(T item)
        {
            Item = item;
            type = item.GetType();
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            serializedText = JsonConvert.SerializeObject(Item);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Item = (T)JsonConvert.DeserializeObject(serializedText, type);
        }
    }
}