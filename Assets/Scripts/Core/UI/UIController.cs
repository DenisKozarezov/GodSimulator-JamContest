using UnityEngine;
using Zenject;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;

namespace Core.UI
{ 
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Animator _playerView;
        [SerializeField]
        private GameObject _cityMiniPanel;
        [SerializeField]
        private TextMeshProUGUI _selectStartCityLabel;
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
            _signalBus.Subscribe<PlayerClickedOnAbilitySignal>(OnPlayerClickedOnAbility);
            _signalBus.Subscribe<PlayerClickedOnCitySignal>(OnPlayerClickedOnCity);
            _signalBus.Subscribe<PlayerCastedTargetAbilitySignal>(OnPlayerUsedTargetAbility);
            _signalBus.Subscribe<PlayerSelectedStartCitySignal>(OnPlayerSelectedStartCity);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<SceneLoadedSignal>(OnSceneLoaded);
            _signalBus.Unsubscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Unsubscribe<TempleDragEndSignal>(OnTempleDragEnd);
            _signalBus.Unsubscribe<CityPointerEnterSignal>(OnCityPointerEnter);
            _signalBus.Unsubscribe<CityPointerExitSignal>(OnCityPointerExit);
            _signalBus.Unsubscribe<PlayerClickedOnAbilitySignal>(OnPlayerClickedOnAbility);
            _signalBus.Unsubscribe<PlayerClickedOnCitySignal>(OnPlayerClickedOnCity);
            _signalBus.Unsubscribe<PlayerCastedTargetAbilitySignal>(OnPlayerUsedTargetAbility);
            _signalBus.Unsubscribe<PlayerSelectedStartCitySignal>(OnPlayerSelectedStartCity);
        }

        private void OnSceneLoaded()
        {
            Cursor.visible = true;
        }
        private void OnPlayerSelectedStartCity()
        {
            Destroy(_selectStartCityLabel.gameObject);
            ShowUI(true);
        }
        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            if (signal.Temple != null)
            {
                SetCursor(CursorType.Target);
                SetSelectionMode(true);
            }
        }
        private void OnTempleDragEnd()
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
        private void OnPlayerClickedOnAbility()
        {
            SetCursor(CursorType.Ability);
            SetSelectionMode(true);
        }
        private void OnPlayerUsedTargetAbility()
        {
            SetCursor(CursorType.Default);
            SetSelectionMode(false);
        }
        private void OnPlayerClickedOnCity(PlayerClickedOnCitySignal signal)
        {
            //var form = Instantiate(_cityMiniPanel, signal.View.transform);
        }

        private void SetCursor(CursorType cursorType)
        {
            Cursor.SetCursor(_cursors[cursorType], _cursorSize, _cursorMode);
        }
        private void SetSelectionMode(bool isSelected)
        {
            _selectionMode = isSelected;
        }
        private void ShowUI(bool isShown)
        {
            _playerView?.SetTrigger(isShown ? "Enter" : "Exit");
        }
    }
}