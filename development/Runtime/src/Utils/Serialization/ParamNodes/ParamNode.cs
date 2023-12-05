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

    [Serializable]
    public abstract class Function<T> : IFunction<T>
    {
        public abstract T Execute();

        public Function()
        {
        }
    }

    [Serializable]
    public class SerTestFunc : IFunction<int>
    {
        public ParamNode<int> OtherParam;
        
        public int Execute()
        {
            return OtherParam.Function.Item.Execute();
        }
    }

    [Serializable]
    public class SerTestFunc2 : IFunction<int>
    {
        public int Execute()
        {
            return 3;
        }
    }
}