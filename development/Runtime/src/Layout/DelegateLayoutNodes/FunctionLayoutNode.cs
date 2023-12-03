namespace FSM.Runtime
{
    public abstract class FunctionLayoutNode : ILayoutNode
    {
        public abstract object LogicObject { get; }
        public ILayoutNode Connection { get; }

        public FunctionLayoutNode(ILayoutNode connection)
        {
            Connection = connection;
        }

        public abstract object ExecuteObject();
    }

    public sealed class FunctionLayoutNode<T> : FunctionLayoutNode
    {
        private readonly IFunction<T> functionObject;
        public override object LogicObject => functionObject;

        public FunctionLayoutNode(ILayoutNode connection, IFunction<T> functionObject) : base(connection)
        {
            this.functionObject = functionObject;
        }

        public override object ExecuteObject() => functionObject.Execute();
    }
}