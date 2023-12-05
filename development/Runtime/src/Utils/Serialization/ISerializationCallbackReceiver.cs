using System.Runtime.Serialization;

namespace FSM.Runtime.Serialization
{
    public interface ISerializationCallbackReceiver
    {
        [OnSerializing]
        void OnSerializing(StreamingContext context);

        [OnDeserializing]
        void OnDeserializingMethod(StreamingContext context);
    }
}