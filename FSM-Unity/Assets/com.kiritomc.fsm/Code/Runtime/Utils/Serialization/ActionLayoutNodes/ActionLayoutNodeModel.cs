using System;

namespace FSM.Runtime.Serialization
{
    [Serializable]
    public class ActionLayoutNodeModel
    {
        public AbstractSerializableType<IAction> Action;
        public AbstractSerializableType<ActionLayoutNodeModel> Connection;

        public ActionLayoutNodeModel() { }

        public ActionLayoutNodeModel(IAction action)
        {
            Action = new AbstractSerializableType<IAction>(action);
        }

        public ActionLayoutNodeModel(IAction action, ActionLayoutNodeModel connection)
        {
            Action = new AbstractSerializableType<IAction>(action);
            Connection = new AbstractSerializableType<ActionLayoutNodeModel>(connection);
        }
    }
}