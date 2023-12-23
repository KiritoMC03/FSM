using System;
using System.Threading.Tasks;
using FSM.Editor.Events;

namespace FSM.Editor
{
    public static class TransitionRequest
    {
        public static Func<Task<TNode>> NewAsync<TNode>(TNode source) where TNode: IVisualNodeWithTransitions
        {
            return RequestFunc;
            Task<TNode> RequestFunc()
            {
                TaskCompletionSource<TNode> completionSource = new TaskCompletionSource<TNode>();
                TransitionRequestEvent<TNode> @event = TransitionRequestEvent<TNode>.GetPooled();
                @event.target = source;
                @event.SetTransitionCallback = node => completionSource.SetResult(node);
                source.SendEvent(@event);
                return completionSource.Task;
            };
        }
    }

    public static class NodeLinkRequest
    {
        public static Func<Task<IVisualNodeWithLinkExit>> NewAsync<TSourceNode>(TSourceNode source) 
            where TSourceNode: VisualNode
        {
            return RequestFunc;
            Task<IVisualNodeWithLinkExit> RequestFunc()
            {
                TaskCompletionSource<IVisualNodeWithLinkExit> completionSource = new TaskCompletionSource<IVisualNodeWithLinkExit>();
                VisualNodeLinkRequestEvent @event = VisualNodeLinkRequestEvent.GetPooled();
                @event.target = source;
                @event.TargetGotCallback = node => completionSource.SetResult(node);
                source.SendEvent(@event);
                return completionSource.Task;
            };
        }
    }
}