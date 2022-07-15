using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class AnimatedDottedLine : MonoBehaviour
    {      
        private LineRenderer _renderer;
        private Material _material;
        private Vector2 _startPosition;
        private Vector2 _endPosition;

        public Vector2 StartPosition
        {
            get => _startPosition;
            set
            {
                _startPosition = value;
                _renderer.SetPosition(0, value);
                IsBackward = _endPosition.x - _startPosition.x < 0;
            }
        }
        public Vector2 EndPosition
        {
            get => _endPosition;
            set
            {
                _endPosition = value;
                _renderer.SetPosition(1, value);
                IsBackward = _endPosition.x - _startPosition.x < 0;
            }
        }

        public bool IsBackward
        {
            get => _material.GetFloat("_IsRight") == 1;
            set => _material.SetFloat("_IsRight", value ? 1 : -1);
        }
        public float LineWidth
        {
            get => _renderer.startWidth;
            set => _renderer.startWidth = _renderer.endWidth = value;
        }

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
            _material = _renderer.material;
        }       
    }
}