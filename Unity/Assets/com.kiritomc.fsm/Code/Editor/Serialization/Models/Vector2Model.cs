using System;
using UnityEngine;

namespace FSM.Editor.Serialization
{
    [Serializable]
    public struct Vector2Model
    {
        public float X;
        public float Y;

        public Vector2Model(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(Vector2Model v) => new Vector2(v.X, v.Y);
        public static explicit operator Vector2Model(Vector2 v) => new Vector2Model(v.x, v.y);
    }
}