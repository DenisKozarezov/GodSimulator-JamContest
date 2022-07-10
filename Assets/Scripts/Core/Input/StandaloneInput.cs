using System;
using UnityEngine;

namespace Core.Input
{
    public class StandaloneInput : IInputSystem
    {
        private PlayerControls _playerControls;

        public Action Escape { get; }
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
    }
}