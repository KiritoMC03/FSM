using UnityEngine;

namespace FSM.Editor.Extensions
{
    public static class Vector2Extensions
    {
        public static bool Approximately(this Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude < 1f;
        }
    }
}