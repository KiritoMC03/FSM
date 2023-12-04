namespace FSM.Runtime
{
    public sealed class ActionLayoutNode : ILayoutNode
    {
        private readonly IAction actionObject;
        public object LogicObject => actionObject;
        public ILayoutNode Connection { get; }

        public void Execute()
        {
            actionObject.Execute();
        }

        public ActionLayoutNode(IAction actionObject, ILayoutNode nextAction)
        {
            this.actionObject = actionObject;
            Connection = nextAction;
        }
    }
}