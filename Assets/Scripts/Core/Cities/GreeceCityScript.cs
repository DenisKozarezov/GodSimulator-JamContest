using System.Collections;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using Zenject;
using TMPro;
using static Core.Models.GameSettingsInstaller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        [SerializeField]
        private SerializableDictionaryBase<GodModel, ushort> _numberOfPriests;
        [SerializeField]
        private SerializableDictionaryBase<GodModel, float> _percentageOfFaithful;
        [SerializeField]
        private GodModel _invader;
        private Temple _temple;
        private Vector3 _startPosition;
        [SerializeField]
        private VirtueModel _testVirtue;
        [SerializeField]
        private bool _increasePassiveFaithful;
        private bool _isDragging;

        private Coroutine _generatePriests;

        public SerializableDictionaryBase<GodModel, float> PercentageOfFaithful => _percentageOfFaithful;
        public bool IsIncreasePassiveFaithful { get { return _increasePassiveFaithful; } set { _increasePassiveFaithful = value; } }

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
                    //StopCoroutine(IncreasePercentageOfFaithful());
                    //BuildTemple();
                    _generatePriests = StartCoroutine(GeneratePriests(_invader));
                    IncreasePercentageOfFaithfulInOtherCities();
                    break;
                case State.CityDestroyed:
                    StopCoroutine(_generatePriests);
                    Interactable = false;
                    break;
            }
        }

        public void AddPriests(GodModel god, ushort value)
        {
            if (!_numberOfPriests.ContainsKey(god))
                _numberOfPriests.Add(god, value);
            else
                _numberOfPriests[god] += value;
        }

        public void DeletePriests(GodModel god, ushort value)
        {
            _numberOfPriests[god] -= value;
        }

        IEnumerator GeneratePriests(GodModel god)
        {
            while (true)
            {
                _numberOfPriests[god] += _growthOfPriests;
                yield return new WaitForSeconds(10f);
            }
        }

        private void IncreasePercentageOfFaithfulInOtherCities()
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, _temple.Range);
            Collider2D selfCollider = GetComponent<Collider2D>();
            List<Collider2D> colliders = colliderArray.ToList();
            colliders.Remove(selfCollider);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<GreeceCityScript>(out GreeceCityScript city))
                {
                    if (city._state == State.CityFree)
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

        public void AddGodToPercentageOfFaithful(GodModel god)
        {
            if (!_percentageOfFaithful.ContainsKey(god))
            {
                _percentageOfFaithful.Add(god, 0);
            }
        }

        IEnumerator IncreasePercentageOfFaithful()
        {
            float templeRate = 0.5f;
            while (_state == State.CityFree)
            {
                foreach (var godKey in _percentageOfFaithful.Keys.ToList())
                {
                    //_percentageOfFaithful[godKey] += templeRate + faithRate;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        private void Start()
        {
            _percentageOfFaithful = new SerializableDictionaryBase<GodModel, float>();
            _startPosition = transform.position;
            if (_state == State.CityWithTemple && _invader != null)
            {
                _numberOfPriests.Add(_invader, 0);
                BuildTemple(_testVirtue);
                _generatePriests = StartCoroutine(GeneratePriests(_invader));
                IncreasePercentageOfFaithfulInOtherCities();
            }
        }

        public void BuildTemple(VirtueModel virtue)
        {
            Temple temple = gameObject.AddComponent<Temple>();
            temple.SetInitialValues(virtue, 10, 5f);
            _temple = temple;
        }

        private void ShowRangeToCities()
        {
            SignalBus.Fire(new PlayerWantToMovingPriestsSignal { City = this, TempleRange = _temple.Range });
        }

        public override void OnMouseDown()
        {
            if (!Interactable) return;

            if (_state == State.CityWithTemple && _invader != null && _invader.ID == 0) {
                _isDragging = true;
                ShowRangeToCities();
            }
        }

        private void OnMouseUp()
        {
            _isDragging = false;
            transform.position = _startPosition;
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
                transform.Translate(mousePosition);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDragging)
            {
                if (other.gameObject.TryGetComponent(out GreeceCityScript toCity))
                {
                    var color = toCity.GetComponent<SpriteRenderer>().color;
                    if (color == Color.green)
                        SignalBus.Fire(new PlayerClickedOnCitySignal { View = this, God = _invader, FromCity = this, ToCity = toCity, NumberOfPriests = _numberOfPriests[_invader] });
                }
            }
        }
    }
}