using UnityEngine;
using Zenject;

namespace Core
{
    public abstract class InteractableView : MonoBehaviour
    {
        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        [Inject]
        public void Contruct(SignalBus signalBus) => _signalBus = signalBus;

        public abstract void OnMouseDown();
        public abstract void OnMouseEnter();
        public abstract void OnMouseExit();
    }
}