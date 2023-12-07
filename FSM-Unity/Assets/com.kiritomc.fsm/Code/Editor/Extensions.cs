using System;

namespace FSM.Editor
{
    public static class Extensions
    {
        public static T With<T>(this T current, Action<T> action) where T : class
        {
            action?.Invoke(current);
            return current;
        }
    }
}