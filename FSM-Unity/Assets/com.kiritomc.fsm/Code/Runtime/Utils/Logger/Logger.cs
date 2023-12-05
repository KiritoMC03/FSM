namespace FSM.Runtime.Utils
{
    public static class Logger
    {
#if UNITY_5_3_OR_NEWER
        public static void Log(string message) => UnityEngine.Debug.Log($"[FSM] {message}");
        public static void LogError(string message) => UnityEngine.Debug.LogError($"[FSM Error] {message}");
#else
        public static void Log(string message) => System.Console.WriteLine($"[FSM] {message}");
        public static void LogError(string message) => System.Console.WriteLine($"[FSM Error] {message}");
#endif
    }
}