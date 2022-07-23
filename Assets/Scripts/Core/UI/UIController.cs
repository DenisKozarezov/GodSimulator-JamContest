using UnityEngine;
using Zenject;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;

namespace Core.UI
{ 
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionaryBase<CursorType, Texture2D> _cursors;
        private bool _selectionMode;
  
        private SignalBus _signalBus;
        private Vector2 _cursorSize;
        private CursorMode _cursorMode;

        [Inject]
        public void Construct(SignalBus signalBus, UISettings settings)
        {
            _signalBus = signalBus;
            _cursorSize = settings.CursorSize * 0.5f;
            _cursorMode = settings.CursorMode;
        }
        private void Awake()
        {
            SetCursor(CursorType.Default);
            _signalBus.Subscribe<SceneLoadedSignal>(OnSceneLoaded);
            _signalBus.Subscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Subscribe<TempleDragEndSignal>(OnTempleDragEnd);
            _signalBus.Subscribe<CityPointerEnterSignal>(OnCityPointerEnter);
            _signalBus.Subscribe<CityPointerExitSignal>(OnCityPointerExit);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<SceneLoadedSignal>(OnSceneLoaded);
            _signalBus.Unsubscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Unsubscribe<TempleDragEndSignal>(OnTempleDragEnd);
            _signalBus.Unsubscribe<CityPointerEnterSignal>(OnCityPointerEnter);
            _signalBus.Unsubscribe<CityPointerExitSignal>(OnCityPointerExit);
        }

        private void OnSceneLoaded()
        {
            Cursor.visible = true;
        }
        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            if (signal.Temple != null)
            {
                SetCursor(CursorType.Target);
                SetSelectionMode(true);
            }
        }
        private void OnTempleDragEnd(TempleDragEndSignal signal)
        {
            SetCursor(CursorType.Default);
            SetSelectionMode(false);            
        }
        private void OnCityPointerEnter(CityPointerEnterSignal signal)
        {
            if (_selectionMode) return;
            
            if (!signal.View.Interactable) SetCursor(CursorType.Disabled);
            else SetCursor(CursorType.Hover);
        }
        private void OnCityPointerExit()
        {
            if (!_selectionMode) SetCursor(CursorType.Default);
        }

        private void SetCursor(CursorType cursorType)
        {
            Cursor.SetCursor(_cursors[cursorType], _cursorSize, _cursorMode);
        }
        private void SetSelectionMode(bool isSelected)
        {
            _selectionMode = isSelected;
        }
    }
}