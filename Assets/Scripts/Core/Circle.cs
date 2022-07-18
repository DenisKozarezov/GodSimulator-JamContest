using UnityEngine;

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

            float x;
            float y;
            float angle = 20f;

            _lineRenderer.positionCount = _segments;
            for (int i = 0; i < _segments; i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                _lineRenderer.SetPosition(i, new Vector3(x, y));

                angle += 360f / _segments;
            }
        }
    }
}