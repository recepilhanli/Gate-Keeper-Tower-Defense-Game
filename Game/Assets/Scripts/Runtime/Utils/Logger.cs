
using System.Diagnostics;

namespace Game.Utils.Logger
{
    //Prevent garbage in game builds
    public static class Debug
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message, string color = "white")
        {
            UnityEngine.Debug.Log($"<color={color}>{message}</color>");
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}