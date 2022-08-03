using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using Unity.Mathematics;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using Zenject;
using Core.Infrastructure;
using Core.Models;
using Core.UI;

namespace Core.Cities
{
    public class CityScript : InteractableView, IEquatable<CityScript>
    {
        enum CityStatus : byte
        {
            Neutral = 0x00,
            Temple = 0x01,
            Destroyed = 0x02
        }

        [Header("City")]
        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private TextMeshPro _priestsCount;
        [SerializeField]
        private SpriteRenderer _icon;
        [SerializeField]
        private PranaView _pranaView;
        [SerializeField]
        private ushort _maxCapacityOfPriests;
        [SerializeField]
        private SerializableDictionaryBase<CityStatus, Sprite> _icons = new SerializableDictionaryBase<CityStatus, Sprite>();

        private bool _interactable = true;
        private bool _destroyed;
        private ICityStrategy _currentStrategy;
        private ushort _priestsAmount;
        private Player _owner;
        private MapController _mapController;

        public ushort PriestsAmount => _priestsAmount;
        public Player Owner => _owner;
        public bool CanBuildTemple
        {
            get => _currentStrategy is not TempleStrategy && _priestsAmount >= 10;
        }
        public override bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (_currentStrategy != null) _currentStrategy.Interactable = value;
            }
        }
        public event Action Destroyed;

        [Inject]
        private void Construct(GameSettings gameSettings, MapController mapControlller)
        {
            _mapController = mapControlller;
            if (gameSettings.CitiesNames.Count > 0)
            {
                string name = gameSettings.CitiesNames.Dequeue();
                _name.text = name;
                gameObject.name = name + " City";
            }
        }

        protected override void Awake()
        {
            base.Awake();
            MapController.RegisterCity(this);

#if UNITY_EDITOR
            Assert.IsNotNull(_name);
            Assert.IsNotNull(_priestsCount);
            Assert.IsNotNull(_pranaView);
#endif
        }
        protected override void Start()
        {
            SetNeutral();
            Interactable = false;
        }
        private void Disable()
        {
            Interactable = false;
            _currentStrategy?.Disable();
        }
        private T SwitchStrategy<T>() where T : MonoBehaviour, ICityStrategy
        {
            if (GetComponent<T>() is T result) return result;

            foreach (MonoBehaviour strategy in GetComponents<ICityStrategy>())
            {
                Destroy(strategy);
            }
            T newStrategy = gameObject.AddComponent<T>();
            newStrategy.Interactable = _interactable;
            return newStrategy;
        }

        public void AddPriests(ushort value)
        {
            _priestsAmount = (ushort)math.min(_priestsAmount + value, _maxCapacityOfPriests);
            _priestsCount.text = _priestsAmount.ToString();
        }
        public void ReducePriests(ushort value)
        {
            _priestsAmount = (ushort)math.max(_priestsAmount - value, 0);
            _priestsCount.text = _priestsAmount.ToString();
        }
        public void SetNeutral()
        {
            NeutralStrategy neutral = SwitchStrategy<NeutralStrategy>();
            neutral.Construct(_pranaView);
            _icon.sprite = _icons[CityStatus.Neutral];
            _currentStrategy = neutral;
        }
        public void BuildTemple(VirtueModel virtue)
        {
            TempleStrategy temple = SwitchStrategy<TempleStrategy>();
            temple.Construct(SignalBus, _mapController);
            temple.SetVirtue(virtue);
            _icon.sprite = _icons[CityStatus.Temple];
            _icon.material.SetColor("_Color", Owner.Color);
            _currentStrategy = temple;
        }
        public void DestroyCity()
        {
            if (_destroyed) return;

            _destroyed = true;
            _icon.sprite = _icons[CityStatus.Destroyed];
            Disable();
            MapController.UnregisterCity(this);
            Destroyed?.Invoke();
        }
        public void SetOwner(Player owner)
        {
            if (owner == null) return;
            _owner = owner;
        }
        public void ClearPriests()
        {
            _priestsAmount = 0;
            _priestsCount.text = _priestsAmount.ToString();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;

            SignalBus.Fire(new PlayerClickedOnCitySignal { View = this });
        }

        public bool Equals(CityScript other)
        {
            return name.Equals(other.name);
        }
    }
}