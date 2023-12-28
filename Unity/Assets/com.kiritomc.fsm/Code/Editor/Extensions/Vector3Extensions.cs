using UnityEngine;

namespace FSM.Editor.Extensions
{
    public static class Vector3Extensions
    {
        public static bool Approximately(this Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude < 1f;
        }
    }
}