namespace FSM.Runtime
{
    public class LogicLayoutNode : ILogicLayoutNode
    {
        public ILogicLayoutNode Input { get; }
        public object LogicObject { get; }

        public LogicLayoutNode(ILogicLayoutNode input, object logicObject)
        {
            Input = input;
            LogicObject = logicObject;
        }
    }

    public interface ILogicLayoutNode
    {
        ILogicLayoutNode Input { get; }
        object LogicObject { get; } // ToDo: maybe bad idea
    }
}