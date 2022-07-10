using Core.Cities;
using UnityEngine;

namespace Core.Infrastructure
{
    public class UISignals : MonoBehaviour
    {
        public struct MovingModeChangedSignal
        {
            public GreeceCityScript City;
            public bool Value;
        }
    }
}
