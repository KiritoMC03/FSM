namespace FSM.Editor
{
    public class EditorState
    {
        public readonly EditorStateProperty<bool> DraggingLocked = new EditorStateProperty<bool>();
    }

    public class EditorStateProperty<T>
    {
        public T Value { get; set; }
    }
}