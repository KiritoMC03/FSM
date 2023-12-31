using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class EditorState
    {
        public readonly EditorStateProperty<bool> IsDirty = new EditorStateProperty<bool>();
        public readonly EditorStateProperty<bool> DraggingLocked = new EditorStateProperty<bool>();
        public readonly EditorStateProperty<Vector3> PointerPosition = new EditorStateProperty<Vector3>();
        public readonly EditorStateProperty<StatesContext> RootContext = new EditorStateProperty<StatesContext>();
        public readonly EditorStateProperty<VisualNodesContext> CurrentContext = new EditorStateProperty<VisualNodesContext>();
        public readonly EditorStateProperty<VisualElement> EditorRoot = new EditorStateProperty<VisualElement>();
    }

    public class EditorStateProperty<T>
    {
        private T value;
        public event Action<T> ValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                ValueChanged?.Invoke(value);
            }
        }
    }
}