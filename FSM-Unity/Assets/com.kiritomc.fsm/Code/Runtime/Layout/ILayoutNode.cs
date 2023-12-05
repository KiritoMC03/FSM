namespace FSM.Runtime
{
    public interface ILayoutNode
    {
        object LogicObject { get; } // ToDo: maybe bad idea
        ILayoutNode Connection { get; }
    }
}