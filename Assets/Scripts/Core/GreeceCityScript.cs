using Core.Infrastructure;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Models;
using System.Collections;
using Zenject;
using static Core.Infrastructure.UISignals;

namespace Core
{
    public class GreeceCityScript : InteractableView
    {
        public enum State
        {
            CityFree,
            CityWithTemple,
            CityDestroyed
        }

        [SerializeField]
        private State _state;
        [SerializeField]
        private byte _growthOfPriests;
        [SerializeField]
        private ushort _numberOfPriests;
        [SerializeField]
        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        [SerializeField]
        private SerializableDictionaryBase<CityModel, sbyte> _relationsToOtherCities;
        [SerializeField]
        private GodModel _invader;
        [SerializeField]
        private Temple _temple;

        public State CurrentState => _state;
        public byte GrowthOfPriests => _growthOfPriests;
        public ushort NumberOfPriests => _numberOfPriests;
        public SerializableDictionaryBase<GodModel, byte> PercentageOfFaithful => _percentageOfFaithful;
        public SerializableDictionaryBase<CityModel, sbyte> RelationsToOtherCities => _relationsToOtherCities;
        public GodModel Invader => _invader;
        public Temple Temple => _temple;

        private Coroutine _generatePriests;

        public void SetState(State state)
        {
            _state = state;
            switch (_state)
            {
                case State.CityWithTemple:
                    _growthOfPriests = 1;
                    _generatePriests = StartCoroutine(GeneratePriests());
                    break;
                case State.CityDestroyed:
                    StopCoroutine(_generatePriests);
                    break;
            }
        }

        public void AddPriests(ushort value)
        {
            _numberOfPriests += value;
        }

        public void DeletePriests(ushort value)
        {
            _numberOfPriests -= value;
        }

        IEnumerator GeneratePriests()
        {
            while (true)
            {
                _numberOfPriests += _growthOfPriests;
                yield return new WaitForSeconds(10f);
            }
        }

        private void Start()
        {
            _percentageOfFaithful = new SerializableDictionaryBase<GodModel, byte>();
            _relationsToOtherCities = new SerializableDictionaryBase<CityModel, sbyte>();
            if (_state == State.CityWithTemple)
                _generatePriests = StartCoroutine(GeneratePriests());
        }

        private void Update()
        {
            Debug.Log(NumberOfPriests);
        }

        public override void OnMouseDown()
        {
            //SignalBus.AbstractFire(new PlayerClickedOnCitySignal { View = this });
            if (_state == State.CityWithTemple) {
                SignalBus.Fire(new MovingModeChangedSignal { City = this, Value = true });
            }
        }

        public void ShowRangeToCities()
        {
            SignalBus.Fire(new PlayerWantToMovingPriestsSignal { City = this, TempleRange = 5f });
        }
    }
}