using System;
using System.Threading.Tasks;
using FSM.Editor.Extensions;

namespace FSM.Editor
{
    public class TransitionsFabric
    {
        public async Task<VisualStateTransition> RouteTransitionAsync<T>(T source, Func<Task<T>> asyncTargetGetter, Predicate<T> checkValid)
            where T : VisualStateNode
        {
            T target = await asyncTargetGetter.Invoke();
            if (target == null || target == source || !checkValid.Invoke(target)) return default;
            return CreateTransition(source, target);
        }

        public VisualStateTransition CreateTransition<T>(T source, T target)
            where T : VisualStateNode
        {
        }

        public void DestroyTransition<T>(T parent, VisualStateTransition transition)
            where T : StateNode
        {
            parent.Remove(transition);
        }
    }
}