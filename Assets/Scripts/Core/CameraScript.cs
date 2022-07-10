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
        private const float ZoomMin = 7;
        private const float ZoomMax = 3;

        [Header("Camera")]
        [SerializeField]
        private float _speed; 

        [Header("Post-processing")]
        [SerializeField]
        private PostProcessVolume _volume;
        [SerializeField]
        private RawImage _fade;

        private IInputSystem _inputSystem;
        private float _zoomVelocity;

        [Inject]
        public void Construct(IInputSystem inputSystem) => _inputSystem = inputSystem;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }
        private void Update()
        {
            if (_inputSystem == null) return;

            UpdateMove();
            UpdateZoom();
        }
         
        private void UpdateMove()
        {
            if (_inputSystem.MousePosition.x >= Screen.width * 0.9f)
            {
                Translate(Vector2.right);
            }
            if (_inputSystem.MousePosition.x <= Screen.width * 0.1f)
            {
                Translate(Vector2.left);
            }
            if (_inputSystem.MousePosition.y >= Screen.height * 0.9f)
            {
                Translate(Vector2.up);
            }
            if (_inputSystem.MousePosition.y <= Screen.height * 0.1f)
            {
                Translate(Vector2.down);
            }
        }
        private void UpdateZoom()
        {
            if (_inputSystem.MouseWheelDelta != 0f)
            {
                float zoom = Mathf.SmoothDamp(_camera.orthographicSize, _camera.orthographicSize + _inputSystem.MouseWheelDelta, ref _zoomVelocity, 0.3f);
                _camera.orthographicSize = Mathf.Clamp(zoom, ZoomMax, ZoomMin);
            }
        }

        private void Translate(Vector3 direction)
        {
            transform.position += direction * _speed * Time.deltaTime;
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
            Color endColor = mode == FadeMode.Off ? Color.black : _fade.color.SetAlpha(0f);
            _fade.DOColor(endColor, time);
        }
    }
}