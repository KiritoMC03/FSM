using System.Runtime.CompilerServices;

namespace FSM.Runtime
{
    public class FsmUpdater //ToDo: rename
    {
        private ConditionSolver conditionSolver = new ConditionSolver();
        
        public void Update(FsmData data) //ToDo: rename
        {
            foreach (IFsmAgent agent in data.Agents)
            {
#if FSM_CATCH
            try
            {
                UpdateAgent(agent);
            }
            catch (System.Exception e)
            {
                Utils.Logger.LogError(e.Message);
            }
#else
                UpdateAgent(agent);
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAgent<T>(T agent) where T: IFsmAgent
        {
            IState currentState = agent.CurrentState;
            if (currentState.OutgoingTransitions != null)
            {
                foreach (ITransition transition in currentState.OutgoingTransitions)
                {
                    if (conditionSolver.Solve(transition.Condition))
                    {
                        currentState.OnExit();
                        currentState = agent.CurrentState = transition.To;
                        currentState.OnEnter();
                    }
                }
            }
            currentState.OnUpdate();
        }
    }
}