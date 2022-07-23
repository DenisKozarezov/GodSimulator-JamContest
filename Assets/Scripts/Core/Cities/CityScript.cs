using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using TMPro;
using Zenject;
using Core.Infrastructure;
using Core.Models;
using Core.UI;
using DG.Tweening;
using static Core.Models.GameSettingsInstaller;

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
        private ICityStrategy _currentStrategy;
        private ushort _priestsAmount;
        private Player _owner;

        public ushort PriestsAmount => _priestsAmount;
        public Player Owner => _owner;

        public override bool Interactable
        {
            get => _interactable;
            protected set
            {
                _interactable = value;
                if (_currentStrategy != null) _currentStrategy.Interactable = value;
            }
        }

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            if (gameSettings.CitiesNames.Count > 0)
            {
                string name = gameSettings.CitiesNames.Dequeue();
                _name.text = name;
                gameObject.name = name + " City";
            }
        }

        private void Awake()
        {
            SignalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            MapController.RegisterCity(this);
        }
        protected override void Start()
        {
            _currentStrategy = GetComponent<ICityStrategy>();
            Interactable = true;
        }
        private void OnDestroy()
        {
            SignalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        }

        private void OnGameStarted()
        {
            if (_pranaView != null)
            {
                DOTween.To(() => 0f, (x) => _pranaView.SetFillAmount(x), 1f, 15f).SetEase(Ease.Linear);
            }
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
            TempleStrategy temple = gameObject.AddComponent<TempleStrategy>();
            temple.SetVirtue(virtue);
            _currentStrategy = temple;
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