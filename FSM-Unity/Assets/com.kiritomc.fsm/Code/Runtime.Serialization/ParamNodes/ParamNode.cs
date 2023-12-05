using System;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class ParamNode<T>
    {
        public AbstractSerializableType<IFunction<T>> Function;

        public ParamNode() { }

        public ParamNode(IFunction<T> function)
        {
            Function = new AbstractSerializableType<IFunction<T>>(function);
        }

        public T Execute() => Function.Item.Execute();
    }
}