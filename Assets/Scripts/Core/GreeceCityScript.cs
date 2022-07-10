using System.Collections;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using Zenject;
using static Core.Infrastructure.UISignals;

namespace Core
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
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private State _state;
        [SerializeField]
        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        [SerializeField]
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
        public override void OnMouseEnter()
        {
           
        }
        public override void OnMouseExit()
        {
         
        }
    }
}