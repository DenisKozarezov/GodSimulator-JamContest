using Core.Cities;

namespace Core.Infrastructure
{
    public interface ISignalInteractableView 
    {
        InteractableView View { get; }
    }

    public struct PlayerClickedOnCitySignal : ISignalInteractableView
    {
        public InteractableView View { set; get; }
        public GreeceCityScript City;
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