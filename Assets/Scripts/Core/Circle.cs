using UnityEngine;
using Unity.Mathematics;

namespace Core.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class Circle : MonoBehaviour
    {
        [SerializeField]
        private int _segments;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        private void Clear()
        {
            _lineRenderer.positionCount = 0;
        }
        public void SetRadius(float radius)
        {
            Clear();

            _lineRenderer.positionCount = _segments;
            
            float angle = 20f;
            for (int i = 0; i < _segments; i++)
            {
                float x = math.sin(math.radians(angle)) * radius;
                float y = math.cos(math.radians(angle)) * radius;

                _lineRenderer.SetPosition(i, new Vector2(x, y));

                angle += 360f / _segments;
            }
        }
    }
}