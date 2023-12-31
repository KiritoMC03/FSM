﻿using System;
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
        
        public void Execute()
        {
            Logger.Log(message);
        }
    }
}