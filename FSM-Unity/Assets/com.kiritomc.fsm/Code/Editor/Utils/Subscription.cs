using System;

namespace FSM.Editor
{
    public class Subscription : IDisposable
    {
        private Action unsubscribeAction;

        public Subscription(Action unsubscribeAction)
        {
            this.unsubscribeAction = unsubscribeAction;
        }

        public void Dispose()
        {
            unsubscribeAction?.Invoke();
            unsubscribeAction = default;
        }
    }
}