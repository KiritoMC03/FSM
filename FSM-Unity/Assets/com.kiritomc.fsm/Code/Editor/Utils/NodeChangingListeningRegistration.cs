using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class NodeChangingListeningRegistration : IDisposable
    {
        private readonly Node node;
        private readonly Action onChanged;

        public NodeChangingListeningRegistration(Node node, Action onChanged)
        {
            this.node = node;
            this.onChanged = onChanged;
            node.RegisterCallback<PointerMoveEvent>(Handler);
        }

        private void Handler(EventBase evt) => onChanged.Invoke();

        public void Dispose()
        {
            node?.UnregisterCallback<PointerMoveEvent>(Handler);
        }
    }
}