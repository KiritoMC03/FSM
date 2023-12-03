namespace FSM.Runtime
{
    public sealed class ConditionLayoutNode : ILayoutNode
    {
        public object LogicObject { get; }
        /// <summary>
        /// Conditions has no connections.
        /// </summary>
        public ILayoutNode Connection { get; } = default;

        public ConditionLayoutNode(object conditionObject)
        {
            LogicObject = conditionObject;
        }
    }
}