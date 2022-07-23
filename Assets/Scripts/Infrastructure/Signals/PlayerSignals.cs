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

    public interface IPlayerCastedAbility { AbilityModel Ability { get; } }
    public struct PlayerClickedOnAbilitySignal { public AbilityModel Ability; }
    public struct PlayerCastedTargetAbilitySignal : IPlayerCastedAbility { public AbilityModel Ability { set; get; } }
    public struct PlayerCastedNonTargetAbilitySignal : IPlayerCastedAbility { public AbilityModel Ability { set; get; } }
}