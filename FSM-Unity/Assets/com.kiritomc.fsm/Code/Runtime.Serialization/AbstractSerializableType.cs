using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class AbstractSerializableType<T>
    {
        [NonSerialized]
        public T Item;
        public Type Type;
        public string SerializedText;

        public AbstractSerializableType()
        {
        }

        public AbstractSerializableType(T item)
        {
            Item = item;
            Type = item.GetType();
        }

        public AbstractSerializableType(object item)
        {
            Item = (T)item;
            Type = item.GetType();
        }

        public AbstractSerializableType(object item, Type itemType)
        {
            Item = (T)item;
            Type = itemType;
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            SerializedText = JsonConvert.SerializeObject(Item);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Item = (T)JsonConvert.DeserializeObject(SerializedText, Type);
        }
    }
}