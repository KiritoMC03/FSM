namespace FSM.Runtime.Utils
{
    public class BinaryNode<T>
    {
        public bool IsLeaf;
        public T Value;
        public BinaryNode<T> Left;
        public BinaryNode<T> Right;
    }
}