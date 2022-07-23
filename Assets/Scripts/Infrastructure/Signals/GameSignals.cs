using UnityEngine.SceneManagement;
using Core.Models;

namespace Core.Infrastructure
{
    public struct SceneLoadedSignal { public Scene Scene; }
    public struct GameStartedSignal { }
    public struct GameApocalypseSignal { }
    public struct PlayerVirtueChangedSignal
    {
        public VirtueModel Virtue;
        public VirtueState State;

        public override string ToString()
        {
            return $"Virtue: {Virtue.DisplayName}. Value: {State.Percent}. Level: {State.Level}.";
        }
    }
}
