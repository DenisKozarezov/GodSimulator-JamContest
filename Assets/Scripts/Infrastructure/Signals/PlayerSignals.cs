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
    public struct PlayerSelectingStartCitySignal { }
    public struct PlayerSelectedStartCitySignal 
    {
        public CityScript View;
    }
    public struct PlayerVictorySignal { }

    public interface IPlayerCastedAbility 
    { 
        Player Player { get; }
        AbilityModel Ability { get; } 
    }
    public struct PlayerClickedOnAbilitySignal
    {
        public Player Player { get; }
        public AbilityModel Ability; 
    }
    public struct PlayerCastedTargetAbilitySignal : IPlayerCastedAbility
    {
        public Player Player { get; set; }
        public AbilityModel Ability { get; set; }
        public CityScript Target;
    }
    public struct PlayerCastedNonTargetAbilitySignal : IPlayerCastedAbility
    {
        public Player Player { get; }
        public AbilityModel Ability { get; set; } 
    }
}