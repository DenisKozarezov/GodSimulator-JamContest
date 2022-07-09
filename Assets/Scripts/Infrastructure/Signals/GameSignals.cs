using System.Collections.Generic;

namespace Core.Infrastructure
{
    public struct GameStartedSignal { }
    public struct PlayerVirtueChangedSignal
    {
        public float War;
        public float Nature;
        public float Love;

        public override string ToString()
        {
            return $"<b><color=red>War</color></b>: {War}, " +
                   $"<b><color=yellow>Nature</color></b>: {Nature}, " +
                   $"<b><color=green>Love</color></b>: {Love}.";
        }
    }
}
