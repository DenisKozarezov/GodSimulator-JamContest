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
            }
        }
        public Vector2 EndPosition
        {
            get => _endPosition;
            set
            {
                _endPosition = value;
                _renderer.SetPosition(1, value);
            }
        }
        public Color Color
        {
            set => _material.SetColor("_Color", value);
        }

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
            _material = _renderer.material;
            _renderer.positionCount = 2;
            _renderer.useWorldSpace = true;
        }       
    }
}