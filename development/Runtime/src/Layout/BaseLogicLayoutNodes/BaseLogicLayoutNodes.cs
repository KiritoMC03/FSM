namespace FSM.Runtime
{
    public class BaseLogicGateNode : ILogicLayoutNode
    {
        /// <summary>
        /// Input is associated as Left node
        /// </summary>
        public ILogicLayoutNode Input { get; }
        public object LogicObject { get; }

        public BaseLogicGateNode(ILogicLayoutNode input, object logicObject)
        {
            Input = input;
            LogicObject = logicObject;
        }
    }
}