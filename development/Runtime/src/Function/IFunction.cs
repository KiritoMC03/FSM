namespace FSM.Runtime
{
    public interface IFunction<out T>
    {
        T Execute();
    }
}