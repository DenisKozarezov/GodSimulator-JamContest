using System.Collections;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using Zenject;
using TMPro;
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
        private byte _growthOfPriests;
        private ushort _numberOfPriests;
        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        private GodModel _invader;

        private Coroutine _generatePriests;

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            if (gameSettings.CitiesNames.Count > 0)
            {
                string name = gameSettings.CitiesNames.Pop();
                _name.text = name;
            }
            gameObject.name = name + " City";
        }

        public void SetState(State state)
        {
            _state = state;
            switch (_state)
            {
                case State.CityWithTemple:
                    //BuildTemple();
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
            if (_state == State.CityWithTemple)
                _generatePriests = StartCoroutine(GeneratePriests());
        }

        public void BuildTemple(VirtueModel virtue)
        {
            Temple temple = gameObject.AddComponent<Temple>();
            temple.SetInitialValues(virtue, 10, 2f);
        }

        public void ShowRangeToCities()
        {
            SignalBus.Fire(new PlayerWantToMovingPriestsSignal { City = this, TempleRange = 5f });
        }

        public override void OnMouseDown()
        {
            if (!Interactable) return;

            if (_state == State.CityWithTemple) {
                SignalBus.Fire(new PlayerClickedOnCitySignal { View = this, NumberOfPriests = _numberOfPriests });
            }
        }
    }
}