﻿using UnityEngine;

namespace FSM.Editor
{
    public class EditorState
    {
        public readonly EditorStateProperty<bool> DraggingLocked = new EditorStateProperty<bool>();
        public readonly EditorStateProperty<Vector3> PointerPosition = new EditorStateProperty<Vector3>();
    }

    public class EditorStateProperty<T>
    {
        public T Value { get; set; }
    }
}