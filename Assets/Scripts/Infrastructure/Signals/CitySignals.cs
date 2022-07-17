using Core.Cities;
using Core.Models;

namespace Core.Infrastructure
{
    public struct TempleDragBeginSignal
    {
        public TempleStrategy Temple;
    }
    public struct TempleDragEndSignal
    {
        public GodModel God;
        public TempleStrategy Temple;
        public CityScript Target;
    }
}

    /*public struct PlayerWantToMovingPriestsSignal
    {
        public GreeceCityScript City;
        public float TempleRange;
    }

    public struct PlayerMovingPriestsSignal
    {
        public GodModel God;
        public GreeceCityScript FromCity;
        public GreeceCityScript ToCity;
        public byte NumberOfPriests;
    }
    */