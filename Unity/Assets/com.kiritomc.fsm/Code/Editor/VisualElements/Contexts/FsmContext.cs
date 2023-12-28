namespace FSM.Editor
{
    public class FsmContext
    {
        public StatesContext StatesContext;

        public FsmContext()
        {
        }

        public FsmContext(StatesContext statesContext)
        {
            StatesContext = statesContext;
        }
    }
}