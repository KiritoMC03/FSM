namespace FSM.Runtime
{
    public class ActionsExecutor
    {
        public void Execute(ILayoutNode actionNode)
        {
            while (actionNode != null)
            {
                if (actionNode is ActionLayoutNode actionLayoutNode)
                {
                    actionLayoutNode.Execute();
                    actionNode = actionNode.Connection;
                }
            }
        }
    }
}