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
        public enum SelectType : byte
        {
            /// <summary>
            /// Can be deselected by mouse exit event.
            /// </summary>
            Weak = 0x00,
            /// <summary>
            /// Cannot be deselected by any UI event.
            /// </summary>
            Strong = 0x01
        }

        [SerializeField]
        private SpriteRenderer _spriteRenderer;  
        private Material _material;

        private SignalBus _signalBus;
        private float _outlineWidth;
        private SelectType _selectType;
        private bool _selected;
        protected SignalBus SignalBus => _signalBus;
        public abstract bool Interactable { get; set; }

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
        public void Select(SelectType type = SelectType.Weak)
        {
            _selectType = type;
            _selected = true;
            SetOutlineWidth(_outlineWidth);
        }
        public void Deselect()
        {
            _selectType = SelectType.Weak;
            _selected = false;
            SetOutlineWidth(0f);
        }
        public abstract void OnPointerClick(PointerEventData eventData);
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (Interactable && !_selected)
            {
                Select();
            }
            SignalBus.Fire(new CityPointerEnterSignal { View = this });
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (Interactable && _selected && _selectType == SelectType.Weak)
            {
                Deselect();
            }
            SignalBus.Fire<CityPointerExitSignal>();
        }
    }
}