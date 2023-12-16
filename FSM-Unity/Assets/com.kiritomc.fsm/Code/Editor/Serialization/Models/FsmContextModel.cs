using System;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public class FsmContextModel
    {
        public StatesContextModel StatesContextModel = new StatesContextModel();

        public FsmContextModel()
        {
        }
    }
}