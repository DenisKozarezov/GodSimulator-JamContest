using UnityEngine;
using Core.Infrastructure;
using TMPro;

namespace Core
{
    public class GreeceCityScript : InteractableView
    {
        [SerializeField]
        private TextMeshPro _name;

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