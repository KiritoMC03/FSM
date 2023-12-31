using System;
using System.Reactive.Disposables;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class OnVisualElementClickedRegistration : ICancelable
    {
        private readonly Subscription subscription;
        public bool IsDisposed { get; private set; }

        public OnVisualElementClickedRegistration(VisualElement target, Action handler, int mouseButton = 0)
        {
            target.RegisterCallback<PointerDownEvent>(Handle);
            subscription = new Subscription(() => target.UnregisterCallback<PointerDownEvent>(Handle));
            void Handle(PointerDownEvent e)
            {
                if (e.button == mouseButton)
                    handler?.Invoke();
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            subscription?.Dispose();
            IsDisposed = true;
        }
    }
}