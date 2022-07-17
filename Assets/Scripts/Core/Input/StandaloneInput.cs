using System;
using UnityEngine;
using Zenject;

namespace Core.Input
{
    public class StandaloneInput : IInputSystem, ITickable
    {
        private PlayerControls _playerControls;

        private float _mouseWheelDelta;

        public Action Escape { set; get; }
        public float MouseWheelDelta => _mouseWheelDelta;
        public Vector2 MousePosition
        {
            get
            {
                if (_playerControls == null) return Vector2.zero;
                return _playerControls.Player.MousePosition.ReadValue<Vector2>();
            }
        }

        public StandaloneInput()
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.Escape.performed += _ => Escape?.Invoke();
            _playerControls.Player.MouseWheelDelta.started += _ =>
            {
                _mouseWheelDelta = _playerControls.Player.MouseWheelDelta.ReadValue<Vector2>().y * 0.01f;
            };
            Enable();
        }
        public void Enable()
        {
            _playerControls.Enable();
        }
        public void Disable()
        {
            _playerControls.Disable();
        }
        void ITickable.Tick()
        {
            float sign = -Mathf.Sign(_mouseWheelDelta);
            if (Mathf.Abs(_mouseWheelDelta) > 1E-2)
            {
                _mouseWheelDelta += Time.deltaTime * sign;
            }
            else _mouseWheelDelta = 0f;
        }
    }
}