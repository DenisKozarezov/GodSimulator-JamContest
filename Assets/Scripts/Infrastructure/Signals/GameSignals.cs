using Core.Models;

namespace Core.Infrastructure
{
    public struct GameStartedSignal { }
    public struct GameApocalypsisSignal { }
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
