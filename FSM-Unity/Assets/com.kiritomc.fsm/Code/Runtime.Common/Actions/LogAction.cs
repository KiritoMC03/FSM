using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Serialization;
using FSM.Runtime.Utils;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class LogAction : IAction
    {
        public ParamNode<string> Message;

        public LogAction(string message)
        {
            Message = new ParamNode<string>(new GetValueFunction<string>(message));
        }

        public LogAction(IFunction<string> getMessageFunc)
        {
            Message = new ParamNode<string>(getMessageFunc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute()
        {
            Logger.Log(Message.Execute());
        }
    }
}