using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using Zenject; 
using Core.Models;
using Core.Infrastructure;

namespace Core.Cities
{
    public abstract class InteractableView : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;  
        private Material _material;

        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        private float _outlineWidth;

        public abstract bool Interactable { get; protected set; }

        [Inject]
        private void Contruct(SignalBus signalBus, UISettings _UISettings)
        {
            _signalBus = signalBus;
            _outlineWidth = _UISettings.OutlineWidth;
        }

        protected virtual void Awake()
        {
            _material = _spriteRenderer.material;

#if UNITY_EDITOR
            Assert.IsNotNull(_spriteRenderer);
#endif
        }
        protected abstract void Start();
        private void SetOutlineWidth(float width)
        {
            _material.SetFloat("_OutlineWidth", width);
        }
        private void SetColor(Color color)
        {
            _material.SetColor("_Color", color);
        }

        public abstract void OnPointerClick(PointerEventData eventData);
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;

            SetOutlineWidth(_outlineWidth);
            SignalBus.Fire(new CityPointerEnterSignal { View = this });
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable) return;

            SetOutlineWidth(0f);
            SignalBus.Fire<CityPointerExitSignal>();
        }
    }
}