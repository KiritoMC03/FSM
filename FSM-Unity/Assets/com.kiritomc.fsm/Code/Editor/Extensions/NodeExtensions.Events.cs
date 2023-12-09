using System;

namespace FSM.Editor.Extensions
{
    public static partial class NodeExtensions
    {
        public static NodeChangingListeningRegistration ListenChanges(this Node node, Action onChanged)
        {
            if (node == null || onChanged == null) return default;
            return new NodeChangingListeningRegistration(node, onChanged);
        }
    }
}