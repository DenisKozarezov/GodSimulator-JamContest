using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using Unity.Mathematics;
using TMPro;
using Zenject;
using Core.Infrastructure;
using Core.Models;
using Core.UI;
using DG.Tweening;

namespace Core.Cities
{
    public class CityScript : InteractableView, IEquatable<CityScript>
    {
        [Header("City")]
        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private TextMeshPro _priestsCount;
        [SerializeField]
        private PranaView _pranaView;
        [SerializeField]
        private ushort _maxCapacityOfPriests;

        private bool _interactable = true;
        private bool _destroyed;
        private ICityStrategy _currentStrategy;
        private ushort _priestsAmount;
        private Player _owner;
        private MapController _mapController;

        public ushort PriestsAmount => _priestsAmount;
        public Player Owner => _owner;
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
            _currentStrategy = GetComponent<ICityStrategy>();
            Interactable = false;
        }
        private void Disable()
        {
            Interactable = false;
            _currentStrategy?.Disable();
        }
        private T SwitchStrategy<T>() where T : MonoBehaviour, ICityStrategy
        {
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
        public void ClearPriests()
        {
            _priestsAmount = 0;
            _priestsCount.text = _priestsAmount.ToString();
        }
        public void BuildTemple(VirtueModel virtue)
        {
            TempleStrategy temple = SwitchStrategy<TempleStrategy>();
            temple.Construct(SignalBus, _mapController);
            temple.SetVirtue(virtue);
            _currentStrategy = temple;
        }
        public void DestroyCity()
        {
            if (_destroyed) return;

            _destroyed = true;
            Disable();
            MapController.UnregisterCity(this);
            Destroyed?.Invoke();
        }
        public void SetOwner(Player owner)
        {
            if (owner == null) return;
            _owner = owner;
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