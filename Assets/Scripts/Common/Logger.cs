using UnityEngine;

namespace Core
{
    public enum LogType : byte
    {
        Default = 0x00,
        Warning = 0x01,
        Critical = 0x02,
        Game = 0x04,
        Signal = 0x08
    }

    public interface ILogger
    {
        void Log(string message, LogType type = LogType.Default);
    }

    public class StandaloneLogger : ILogger
    {
        public void Log(string message, LogType type)
        {
#if UNITY_EDITOR
            switch (type)
            {
                case LogType.Default:
                    Debug.Log($"<b>[LOG]: {message}</b>");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"<b><color=yellow>[WARNING]</color></b>: {message}");
                    break;
                case LogType.Critical:
                    Debug.LogError($"<b><color=red>[CRITICAL]</color></b>: {message}");
                    break;
                case LogType.Game:
                    Debug.Log($"<b><color=green>[GAME]</color></b>: {message}");
                    break;
                case LogType.Signal:
                    Debug.Log($"<b><color=green>[SIGNAL]</color></b>: Fired <b><color=yellow>{message}</color></b>.");
                    break;
            }
#endif
        }
    }
}
