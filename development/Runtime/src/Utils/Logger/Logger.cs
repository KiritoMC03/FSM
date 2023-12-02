using System;

namespace Runtime.Utils
{
    public class Logger
    {
#if UNITY_ENGINE
        public static void Log(string message) => Debug.Log($"[FSM] {message}");
        public static void LogError(string message) => Debug.LogError($"[FSM Error] {message}");
#else
        public static void Log(string message) => Console.WriteLine($"[FSM] {message}");
        public static void LogError(string message) => Console.WriteLine($"[FSM Error] {message}");
#endif
    }
}