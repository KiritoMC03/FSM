using System.Runtime.Serialization;

namespace Runtime.Utils.Serialization
{
    public interface ISerializationCallbackReceiver
    {
        [OnSerializing]
        void OnSerializing(StreamingContext context);

        [OnDeserializing]
        void OnDeserializingMethod(StreamingContext context);
    }
}