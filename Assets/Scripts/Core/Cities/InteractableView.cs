using UnityEngine;
using UnityEngine.EventSystems;
using Zenject; 
using Core.Models;

namespace Core.Cities
{
    public abstract class InteractableView : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;     

        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        private bool _isHover;
        private float _outlineWidth;

        public abstract bool Interactable { get; set; }

        [Inject]
        public void Contruct(SignalBus signalBus, UISettings _UISettings)
        {
            _signalBus = signalBus;
            _outlineWidth = _UISettings.OutlineWidth;
        }

        protected abstract void Start();
        private void SetOutlineWidth(float width)
        {
            _spriteRenderer.material.SetFloat("_OutlineWidth", width);
        }
        private void SetColor(Color color)
        {
            _spriteRenderer.material.SetColor("_Color", color);
        }

        public abstract void OnPointerClick(PointerEventData eventData);
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;

            _isHover = true;
            SetOutlineWidth(_outlineWidth);
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable) return;

            _isHover = false;
            SetOutlineWidth(0f);
        }
    }
}