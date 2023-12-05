using System;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class NothingAction : IAction
    {
        public void Execute()
        {
        }
    }
}