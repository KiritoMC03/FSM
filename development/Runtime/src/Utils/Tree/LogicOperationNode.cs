namespace FSM.Runtime.Utils
{
    public sealed class LogicOperationNode : IPoolable
    {
        public bool IsLeaf;
        public bool Value;
        public LogicOperationNode Left;
        public LogicOperationNode Right;
        public Operator Operator;

        public LogicOperationNode()
        {
        }

        public LogicOperationNode(Operator @operator, LogicOperationNode left, LogicOperationNode right)
        {
            IsLeaf = false;
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public LogicOperationNode(Operator @operator, LogicOperationNode left)
        {
            IsLeaf = false;
            Operator = @operator;
            Left = left;
            Right = default;
        }

        public void Reset()
        {
            IsLeaf = false;
            Value = default;
            Left = default;
            Right = default;
        }
    }
}