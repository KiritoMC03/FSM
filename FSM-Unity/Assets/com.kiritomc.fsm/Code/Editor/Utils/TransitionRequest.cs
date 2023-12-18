using System;
using System.Threading.Tasks;
using FSM.Editor.Events;

namespace FSM.Editor
{
    public static class TransitionRequest
    {
        public static Func<Task<TNode>> NewAsync<TNode>(TNode target) where TNode: IVisualNodeWithTransitions
        {
            return RequestFunc;
            Task<TNode> RequestFunc()
            {
                TaskCompletionSource<TNode> completionSource = new TaskCompletionSource<TNode>();
                TransitionRequestEvent<TNode> @event = TransitionRequestEvent<TNode>.GetPooled();
                @event.target = target;
                @event.SetTransitionCallback = node => completionSource.SetResult(node);
                target.SendEvent(@event);
                return completionSource.Task;
            };
        }
    }
}