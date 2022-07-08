using Core.Infrastructure;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Models;
using System.Collections;

namespace Core
{
    public class GreeceCityScript : InteractableView
    {
        public enum State

        private void Start()
        {
            CityWithoutTemple,
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

        public State CurrentState => _state;
        public byte GrowthOfPriests => _growthOfPriests;
        public ushort NumberOfPriests => _numberOfPriests;
        public SerializableDictionaryBase<GodModel, byte> PercentageOfFaithful => _percentageOfFaithful;
        public SerializableDictionaryBase<CityModel, sbyte> RelationsToOtherCities => _relationsToOtherCities;
        public GodModel Invader => _invader;

        private Coroutine _generatePriests;

        public void SetState(State state)
        {
            _state = state;
            switch (_state)
            {
                case State.CityWithTemple:
                    _growthOfPriests = 5;
                    _generatePriests = StartCoroutine(GeneratePriests());
                    break;
                case State.CityDestroyed:
                    StopCoroutine(_generatePriests);
                    break;
            }
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
            SignalBus.AbstractFire(new PlayerClickedOnCitySignal { View = this });
        }
    }
}