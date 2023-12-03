namespace FSM.Runtime
{
    public class BaseGateNode : ILayoutNode
    {
        /// <summary>
        /// Input is associated as Left node
        /// </summary>
        public ILayoutNode Connection { get; }
        public object LogicObject { get; }

        public BaseGateNode(object logicObject, ILayoutNode connection)
        {
            LogicObject = logicObject;
            Connection = connection;
        }
    }
}