using UnityEngine;

namespace Core.UI
{
    public class UIContoller : MonoBehaviour
    {
        [SerializeField]
        private bool _movingMode;

        public bool MovingMode => _movingMode;
    }
}