using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using Core.Input;

namespace Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraScript : MonoBehaviour
    {
        private Camera _camera;
        private float ZoomMin;
        private const float ZoomMax = 3;

        [Header("Camera")]
        [SerializeField]
        private float _speed;
        [SerializeField, Range(0f, 1f)]
        private float _zoomInertia;
        [SerializeField, Range(0f, 1f)]
        private float _moveInertia;
        [SerializeField]
        private Vector2 _constraintsBox;

        [Header("Post-processing")]
        [SerializeField]
        private PostProcessVolume _volume;
        [SerializeField]
        private RawImage _fade;

        private IInputSystem _inputSystem;
        private float _zoomVelocity;
        private Vector3 _moveVelocity;
        private Vector2 _startPosition;

        private float Ratio => GetBounds().x / _constraintsBox.x;

        [Inject]
        public void Construct(IInputSystem inputSystem) => _inputSystem = inputSystem;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _startPosition = transform.position;
            ZoomMin = _camera.orthographicSize + _camera.orthographicSize * (1 - Ratio);
        }
        private void Update()
        {
            if (_inputSystem == null) return;

            UpdateMove();
            UpdateZoom();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector2 position = _startPosition - _constraintsBox * 0.5f;
            UnityEditor.Handles.DrawSolidRectangleWithOutline(new Rect(position, _constraintsBox), Color.green.WithAlpha(0.03f), Color.green);
        }
#endif

        private void UpdateMove()
        {
            Vector2 direction = Vector2.zero;

            if (_inputSystem.MousePosition.x >= Screen.width * 0.9f)
            {
                direction += Vector2.right;
            }
            if (_inputSystem.MousePosition.x <= Screen.width * 0.1f)
            {
                direction += Vector2.left;
            }
            if (_inputSystem.MousePosition.y >= Screen.height * 0.95f)
            {
                direction += Vector2.up;
            }
            if (_inputSystem.MousePosition.y <= Screen.height * 0.05f)
            {
                direction += Vector2.down;
            }
            Translate(direction * 10f);
            transform.position = ClampCameraPosition();
        }
        private void UpdateZoom()
        {
            if (_inputSystem.MouseWheelDelta != 0f)
            {
                float zoom = Mathf.SmoothDamp(0f, _inputSystem.MouseWheelDelta, ref _zoomVelocity, _zoomInertia);
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoom, ZoomMax, ZoomMin);
            }
        }
        private Vector2 GetBounds()
        {
            float height = _camera.orthographicSize * 2f;
            float width = height * _camera.aspect;
            return new Vector2(width, height);
        }    
        private Vector3 ClampCameraPosition()
        {
            Vector2 bounds = GetBounds();

            float minX = _startPosition.x - _constraintsBox.x * 0.5f + bounds.x * 0.5f;
            float maxX = _startPosition.x + _constraintsBox.x * 0.5f - bounds.x * 0.5f;
            float minY = _startPosition.y - _constraintsBox.y * 0.5f + bounds.y * 0.5f;
            float maxY = _startPosition.y + _constraintsBox.y * 0.5f - bounds.y * 0.5f;

            float newX = Mathf.Clamp(transform.position.x, minX, maxX);
            float newY = Mathf.Clamp(transform.position.y, minY, maxY);
            return new Vector3(newX, newY, transform.position.z);
        }

        private void Translate(Vector3 direction)
        {
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + direction, ref _moveVelocity, _moveInertia, _speed);
        }
        private T GetSettings<T>() where T : PostProcessEffectSettings
        {
            if (_volume.profile.TryGetSettings<T>(out var setting))
            {
                return setting;
            }
            return null;
        }
        public void Fade(FadeMode mode, float time)
        {
            _fade.DOFade(mode == FadeMode.Off ? 1f : 0f, time).SetEase(Ease.Linear);
        }
        public void Shake(float time, float strength = 1f)
        {
            transform.DOShakePosition(time, strength);
        }
    }
}