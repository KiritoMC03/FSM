using System;
using FSM.Runtime.Serialization;
using FSM.Runtime.Utils;

namespace FSM.Runtime.Common
{
    [Serializable]
    public class NothingAction : IAction
    {
        public ParamNode<int> ParamNode;
        
        public void Execute()
        {
            Logger.Log($"Result: {ParamNode?.Execute()}");
        }
    }
}