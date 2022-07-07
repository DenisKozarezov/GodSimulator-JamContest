using UnityEngine.EventSystems;
using Core.Infrastructure;

namespace Core
{
    public class GreeceCityScript : InteractableView
    {
        private void Start()
        {

        }
        private void Update()
        {

        }
        public override void OnMouseDown()
        {
            SignalBus.AbstractFire(new PlayerClickedOnCitySignal { View = this });
        }
    }
}