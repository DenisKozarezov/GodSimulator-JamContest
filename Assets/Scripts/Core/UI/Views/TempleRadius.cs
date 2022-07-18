using UnityEngine;

namespace Core.UI
{
    public class TempleRadius : MonoBehaviour
    {
        private Transform _transform;

        public void SetRange(float range)
        {
            _transform.localScale = new Vector2(range * 7, range * 7);
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
    }
}