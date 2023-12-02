using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FSM.Runtime
{
    public struct FsmData //ToDo: rename
    {
        public readonly IEnumerable<IState> States;
        public readonly IEnumerable<ITransition> Transitions;

        public FsmData(
            IEnumerable<IState> states, 
            IEnumerable<ITransition> transitions)
        {
            this.States = states;
            this.Transitions = transitions;
        }
    }

    public class FsmUpdater //ToDo: rename
    {
        private TransitionUpdater transitionUpdater;
        private StateUpdater stateUpdater;

        public void Update(FsmData data) //ToDo: rename
        {
            foreach (var transition in data.Transitions)
            {
                transitionUpdater.Update(transition);
            }

            foreach (var state in data.States)
            {
                stateUpdater.Update(state);
            }
        }
    }

    public class TransitionUpdater
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransitionUpdateResult Update<T>(T transition) where T: ITransition
        {
            return default;
        }
    }

    public class StateUpdater
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update<T>(T state) where T: IState
        {
#if FSM_CATCH
            try
            {
                state.OnUpdate();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#else
            state.OnUpdate();
#endif
        }
    }
}