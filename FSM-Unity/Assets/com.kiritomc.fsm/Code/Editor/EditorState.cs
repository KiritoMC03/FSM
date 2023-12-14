﻿using System;
using UnityEngine;

namespace FSM.Editor
{
    public class EditorState
    {
        public readonly EditorStateProperty<bool> DraggingLocked = new EditorStateProperty<bool>();
        public readonly EditorStateProperty<Vector3> PointerPosition = new EditorStateProperty<Vector3>();
        public readonly EditorStateProperty<StatesContext> RootContext = new EditorStateProperty<StatesContext>();
        public readonly EditorStateProperty<Context> CurrentContext = new EditorStateProperty<Context>();
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