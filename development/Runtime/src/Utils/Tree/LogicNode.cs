namespace FSM.Runtime.Utils
{
    public class LogicNode : IPoolable
    {
        public bool IsLeaf;
        public bool Value;
        public Operator Operator;
        public LogicNode Left;
        public LogicNode Right;

        public LogicNode()
        {
        }

        public LogicNode(bool value)
        {
            IsLeaf = true;
            Value = value;
        }

        public LogicNode(Operator @operator, LogicNode left, LogicNode right)
        {
            IsLeaf = false;
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public LogicNode(Operator @operator, LogicNode left)
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
            Operator = default;
            Left = default;
            Right = default;
        }
    }
}