using Core.Models;

namespace Core.Infrastructure
{
    public struct PlayerVictorySignal { }

    public interface IPlayerUsedAbility { AbilityModel Ability { get; } }
    public struct PlayerUsedTargetAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
    public struct PlayerUsedNonTargetAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
    public struct PlayerUsedAreaAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
}