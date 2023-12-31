using UnityEngine;

namespace FSM.Editor
{
    public static class Keys
    {
        public const int DragNodeMouseButton = 0;
        public const KeyCode CreateNode = KeyCode.Space;
        public const KeyCode DeleteNode = KeyCode.Delete;

        public static readonly KeyCode[] ApplyNodeRename = new [] { KeyCode.Return, KeyCode.KeypadEnter };
    }
}