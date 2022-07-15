using UnityEngine;
using Core.Cities;

namespace Core.Infrastructure
{
    public struct PlayerClickedOnCitySignal
    {
        public CityScript View { set; get; }
    }
    public struct TempleDragBeginSignal
    {
        public TempleStrategy Temple { get; set; }
    }
    public struct TempleDragEndSignal
    {
        public TempleStrategy Temple { get; set; }
        public CityScript Target { get; set; }
    }

    public struct PlayerWantToMovingPriestsSignal
    {
        public Transform Transform;
        public float TempleRange;
    }
}