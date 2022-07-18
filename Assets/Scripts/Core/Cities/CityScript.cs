using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Zenject;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using DG.Tweening;
using static Core.Models.GameSettingsInstaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Core.Cities
{
    public class CityScript : InteractableView, IEquatable<CityScript>
    {
        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private PranaView _pranaView;

        private float _timer;
        private bool _interactable = true;
        private ICityStrategy _currentStrategy;
        [SerializeField]
        private byte _maxCapacityOfPriests;
        [SerializeField]
        private SerializableDictionaryBase<GodModel, ushort> _numberOfPriests;
        private SerializableDictionaryBase<GodModel, float> _percentageOfFaithful;
        [SerializeField]
        private GodModel _invader;
        private bool _increasePassiveFaithful;

        public ICityStrategy CurrentStrategy => _currentStrategy;
        public byte MaxCapacityOfPriests => _maxCapacityOfPriests;
        public SerializableDictionaryBase<GodModel, ushort> NumberOfPriests => _numberOfPriests;
        public GodModel Invader => _invader;
        public bool IsIncreasePassiveFaithful { get { return _increasePassiveFaithful; } set { _increasePassiveFaithful = value; } }

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

        protected override void Start()
        {
            _currentStrategy = GetComponent<ICityStrategy>();

            _numberOfPriests = new SerializableDictionaryBase<GodModel, ushort>();
            if (_currentStrategy is TempleStrategy)
            {
                if (!_numberOfPriests.ContainsKey(_invader))
                {
                    _numberOfPriests.Add(_invader, 0);
                }
            }

            _percentageOfFaithful = new SerializableDictionaryBase<GodModel, float>();
            Interactable = true;

            if (_pranaView != null)
            {
                DOTween.To(() => 0f, (x) => _pranaView.SetFillAmount(x), 1f, 15f).SetEase(Ease.Linear);
            }
            MapController.RegisterCity(this);
        }

        public void AddGodToPercentageOfFaithful(GodModel god)
        {
            if (!_percentageOfFaithful.ContainsKey(god))
            {
                _percentageOfFaithful.Add(god, 0);
            }
        }

        public IEnumerator IncreasePercentageOfFaithful()
        {
            float templeRate = 0.5f;
            while (CurrentStrategy is NeutralStrategy)
            {
                var total = _percentageOfFaithful.Values.ToList().Sum(x => x);
                foreach (var godKey in _percentageOfFaithful.Keys.ToList())
                {
                    if (total >= 100f)
                        yield break;
                    _percentageOfFaithful[godKey] += templeRate;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void IncreasePercentageOfFaithfulInOtherCities()
        {
            TempleStrategy temple = GetComponent<TempleStrategy>();
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, temple.Range);
            Collider2D selfCollider = GetComponent<Collider2D>();
            IEnumerable<Collider2D> colliders = from collider in colliderArray
                         where collider != selfCollider
                         select collider;
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out CityScript city))
                {
                    if (city.CurrentStrategy is NeutralStrategy)
                    {
                        if (!city.IsIncreasePassiveFaithful)
                        {
                            city.IsIncreasePassiveFaithful = true;
                            city.AddGodToPercentageOfFaithful(_invader);
                            StartCoroutine(city.IncreasePercentageOfFaithful());
                        }
                        else
                        {
                            city.AddGodToPercentageOfFaithful(_invader);
                        }
                    }
                }
            }
        }

        public void AddPriests(GodModel god, ushort value)
        {
            if (!_numberOfPriests.ContainsKey(god))
                _numberOfPriests.Add(god, 0);

            _numberOfPriests[god] = (ushort)Math.Min(_numberOfPriests[god] + value, _maxCapacityOfPriests);
        }

        public void ReducePriests(GodModel god, ushort value)
        {
            _numberOfPriests[god] = (ushort)Math.Max(_numberOfPriests[god] - value, 0);
        }

        public void BuildTemple(VirtueModel virtue)
        {
            TempleStrategy temple = gameObject.AddComponent<TempleStrategy>();
            temple.SetVirtue(virtue);
            _currentStrategy = temple;

            if (IsIncreasePassiveFaithful)
                IsIncreasePassiveFaithful = false;
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