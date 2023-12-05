using System;
using System.Runtime.CompilerServices;
using FSM.Runtime.Utils;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class LogAction : IAction
    {
        private readonly string message;

        public LogAction(string message)
        {
            this.message = message;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute()
        {
            Logger.Log(message);
        }
    }
}