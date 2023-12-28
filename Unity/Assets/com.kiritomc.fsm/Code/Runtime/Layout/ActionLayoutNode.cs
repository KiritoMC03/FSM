namespace FSM.Runtime
{
    public sealed class ActionLayoutNode : ILayoutNode
    {
        private IAction actionObject;

        public object LogicObject => actionObject;
        public ILayoutNode Connection { get; set; }

        public void Execute()
        {
            actionObject.Execute();
        }

        public void SetAction(IAction actionObject)
        {
            this.actionObject = actionObject;
        }

        public ActionLayoutNode()
        {
        }

        public ActionLayoutNode(IAction actionObject)
        {
            this.actionObject = actionObject;
        }

        public ActionLayoutNode(IAction actionObject, ILayoutNode nextAction)
        {
            this.actionObject = actionObject;
            Connection = nextAction;
        }
    }
}