using UnityEngine;
using Zenject;

namespace Core
{
    public abstract class InteractableView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        protected bool _isHover;
        public bool Interactable { get; set; } = true;

        [Inject]
        public void Contruct(SignalBus signalBus) => _signalBus = signalBus;

        private void SetOutlineWidth(float width)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            _spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_OutlineWidth", width);
            _spriteRenderer.SetPropertyBlock(mpb);
        }

        public abstract void OnMouseDown();
        private void OnMouseEnter()
        {
            if (!Interactable) return;

            _isHover = true;
            SetOutlineWidth(10f);
        }
        private void OnMouseExit()
        {
            if (!Interactable) return;

            _isHover = false;
            SetOutlineWidth(0f);
        }
    }
}