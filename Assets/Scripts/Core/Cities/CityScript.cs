using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using TMPro;
using Zenject;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
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
        private byte _maxCapacityOfPriests;

        private bool _interactable = true;
        private ICityStrategy _currentStrategy;
        [SerializeField]
        private SerializableDictionaryBase<GodModel, ushort> _numberOfPriests;
        [SerializeField]
        private GodModel _owner;

        public ushort PriestsAmount
        {
            get
            {
                if (_owner == null) return 0;
                if (_numberOfPriests.TryGetValue(_owner, out ushort amount))
                {
                    return amount;
                }
                return 0;
            }
        }
        public GodModel Owner => _owner;

        public override bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (_currentStrategy != null) _currentStrategy.Interactable = value;
            }
        }

        [Inject]
        public void Construct(GameSettings gameSettings)
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
            MapController.RegisterCity(this);
        }
        protected override void Start()
        {
            _currentStrategy = GetComponent<ICityStrategy>();

            _numberOfPriests = new SerializableDictionaryBase<GodModel, ushort>();

            Interactable = true;

            if (_pranaView != null)
            {
                DOTween.To(() => 0f, (x) => _pranaView.SetFillAmount(x), 1f, 15f).SetEase(Ease.Linear);
            }
        }

        public void AddPriests(GodModel owner, ushort value)
        {
            if (!_numberOfPriests.ContainsKey(owner))
                _numberOfPriests.Add(owner, 0);

            _numberOfPriests[owner] = (ushort)math.min(_numberOfPriests[owner] + value, _maxCapacityOfPriests);
            _priestsCount.text = _numberOfPriests[owner].ToString();
        }
        public void ReducePriests(GodModel owner, ushort value)
        {
            if (_numberOfPriests.ContainsKey(owner))
            {
                _numberOfPriests[owner] = (ushort)math.max(_numberOfPriests[owner] - value, 0);
                _priestsCount.text = _numberOfPriests[owner].ToString();
            }
        }
        public void ClearPriests()
        {
            foreach (var priests in _numberOfPriests)
            {
                ReducePriests(priests.Key, priests.Value);
            }
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