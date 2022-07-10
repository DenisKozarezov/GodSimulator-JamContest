using UnityEngine;
using static Core.Infrastructure.UISignals;

namespace Core.UI
{
    public class UIContoller : MonoBehaviour
    {
        [SerializeField]
        private bool _movingMode;

        public bool MovingMode => _movingMode;

        public void SetMovingMode(MovingModeChangedSignal movingModeChangedSignal)
        {
            _movingMode = movingModeChangedSignal.Value;
            if (_movingMode == true)
                movingModeChangedSignal.City.ShowRangeToCities();
        }
    }
}