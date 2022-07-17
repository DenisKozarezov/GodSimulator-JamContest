using Core.Cities;
using Core.Models;

namespace Core.Infrastructure
{
    public struct PlayerClickedOnCitySignal
    {
        public CityScript View;
    }
    public struct PlayerMovingPriestsSignal
    {
        public TempleStrategy Temple;
        public CityScript Target;
        public ushort PriestsAmount;
        public float Duration;
    }
    public struct PlayerVictorySignal { }

    public interface IPlayerUsedAbility { AbilityModel Ability { get; } }
    public struct PlayerUsedTargetAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
    public struct PlayerUsedNonTargetAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
    public struct PlayerUsedAreaAbilitySignal : IPlayerUsedAbility { public AbilityModel Ability { set; get; } }
}