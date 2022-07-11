using Core.Models;
using UnityEngine;
using Zenject;

namespace Core.Cities
{
    public abstract class InteractableView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        protected bool _isHover;
        protected bool _selected;
        private float _outlineWidth;
        public bool Interactable { get; set; } = true;

        [Inject]
        public void Contruct(SignalBus signalBus, UISettings _UISettings)
        {
            _signalBus = signalBus;
            _outlineWidth = _UISettings.OutlineWidth;
        }

        private void SetOutlineWidth(float width)
        {
            _spriteRenderer.material.SetFloat("_OutlineWidth", width);
        }

        public abstract void OnMouseDown();
        private void OnMouseEnter()
        {
            if (!Interactable) return;

            _isHover = true;
            SetOutlineWidth(_outlineWidth);
        }
        private void OnMouseExit()
        {
            if (!Interactable) return;

            _isHover = false;
            SetOutlineWidth(0f);
        }
    }
}