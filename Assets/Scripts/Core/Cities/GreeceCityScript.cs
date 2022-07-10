using System.Collections;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using Zenject;
using TMPro;
using static Core.Infrastructure.UISignals;
using static Core.Models.GameSettingsInstaller;

namespace Core.Cities
{
    public class GreeceCityScript : InteractableView
    {
        public enum State : byte
        {
            CityFree = 0x00,
            CityWithTemple = 0x01,
            CityDestroyed = 0x02
        }

        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private State _state;

        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        private SerializableDictionaryBase<CityModel, sbyte> _relationsToOtherCities;

        private byte _growthOfPriests;
        private ushort _numberOfPriests;
        private GodModel _invader;
        private Temple _temple;

        public State CurrentState => _state;
        public byte GrowthOfPriests => _growthOfPriests;
        public ushort NumberOfPriests => _numberOfPriests;
        public SerializableDictionaryBase<GodModel, byte> PercentageOfFaithful => _percentageOfFaithful;
        public SerializableDictionaryBase<CityModel, sbyte> RelationsToOtherCities => _relationsToOtherCities;

        private Coroutine _generatePriests;

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            string name = gameSettings.CitiesNames.Pop();
            _name.text = name;
            gameObject.name = name + " City";
        }

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
                    Interactable = false;
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

        public void ShowRangeToCities()
        {
            SignalBus.Fire(new PlayerWantToMovingPriestsSignal { City = this, TempleRange = 5f });
        }
        public override void OnMouseDown()
        {
            if (!Interactable) return;

            SignalBus.AbstractFire(new PlayerClickedOnCitySignal { View = this });
            if (_state == State.CityWithTemple) {
                SignalBus.Fire(new MovingModeChangedSignal { City = this, Value = true });
            }
        }
    }
}