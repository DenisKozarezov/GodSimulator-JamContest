namespace Core.Infrastructure
{
    public interface ISignalInteractableView 
    {
        InteractableView View { get; }
    }

    public struct PlayerClickedOnCitySignal : ISignalInteractableView
    {
        public InteractableView View { set; get; }
    }
}