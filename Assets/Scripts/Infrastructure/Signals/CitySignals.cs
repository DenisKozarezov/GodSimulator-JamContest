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
    }
    public struct TempleDragBeginSignal : ISignalInteractableView
    {
        public GreeceCityScript View { get; set; }
    }
    public struct TempleDragEndSignal : ISignalInteractableView
    {
        public GreeceCityScript View { get; set; }
        public GreeceCityScript Target { get; set; }
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