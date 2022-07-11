using Core.Cities;

namespace Core.Infrastructure
{
    public interface ISignalInteractableView 
    {
        GreeceCityScript View { get; }
    }

    public struct PlayerClickedOnCitySignal : ISignalInteractableView
    {
        public GreeceCityScript View { set; get; }
        public ushort NumberOfPriests;
    }

    public struct PlayerWantToMovingPriestsSignal
    {
        public GreeceCityScript City;
        public float TempleRange;
    }

    public struct PlayerMovingPriestsSignal
    {
        public GreeceCityScript FromCity;
        public GreeceCityScript ToCity;
        public byte NumberOfPriests;
    }
}