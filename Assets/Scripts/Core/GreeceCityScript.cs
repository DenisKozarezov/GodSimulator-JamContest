using Core.Infrastructure;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Models;
using System.Collections;
using Zenject;

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
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private State _state;
        private byte _growthOfPriests;
        private ushort _numberOfPriests;
        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        private SerializableDictionaryBase<CityModel, sbyte> _relationsToOtherCities;
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
                    //BuildTemple();
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
            DrawOutline();
        }

        private void DrawOutline()
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            _spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetColor("_OutlineColor", Color.green);
            _spriteRenderer.SetPropertyBlock(mpb);
        }
        public override void OnMouseDown()
        {
            if (_state == State.CityWithTemple) {
                SignalBus.Fire(new PlayerClickedOnCitySignal { View = this, City = this, NumberOfPriests = _numberOfPriests });
            }
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

        public override void OnMouseEnter()
        {
        }

        public override void OnMouseExit()
        {
        }
    }
}