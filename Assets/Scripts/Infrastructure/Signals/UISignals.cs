using Core.Cities;

namespace Core.Infrastructure
{
    public struct SelectionModeChangedSignal
    {
        public bool Value;
    }
    public struct CityPointerEnterSignal { public InteractableView View; }
    public struct CityPointerExitSignal { }
}
