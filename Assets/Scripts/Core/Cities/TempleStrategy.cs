using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Core.Models;
using Core.Infrastructure;

namespace Core.Cities
{
    public class TempleStrategy : MonoBehaviour, ICityStrategy,
        IBeginDragHandler, IEndDragHandler, IEquatable<TempleStrategy>
    {
        [Header("Temple")]
        [SerializeField, Min(0f)]
        private float _minRange;
        [SerializeField, Min(0f)]
        private float _priestsRate;
        [SerializeField]
        private byte _growthOfPriests;

        private SignalBus _signalBus;
        private MapController _mapController;
        private TempleRangeDecorator _rangeDecorator;
        private VirtueModel _virtue;
        private CityScript _city;
        private Coroutine _generatePriests;

        private bool _dragging;
        public bool Interactable { get; set; }

        public CityScript City => _city;
        public VirtueModel Virtue => _virtue;
        public float MinRange => _minRange;

        [Inject]
        public void Construct(SignalBus signalBus, MapController mapController)
        {
            _signalBus = signalBus;
            _mapController = mapController;
        }

        private void Start()
        {
            _city = GetComponent<CityScript>();
            _city.AddPriests(_city.Invader, 0);

            _rangeDecorator = new TempleRangeVirtueLevelDecorator(this);
            _generatePriests = StartCoroutine(GeneratePriests());
            IncreasePercentageOfFaithfulInOtherCities();
        }

        private void IncreasePercentageOfFaithfulInOtherCities()
        {
            IEnumerable<NeutralStrategy> cities = _mapController.SelectByDistance<NeutralStrategy>(
                city => city != this, 
                transform.position, 
                GetRange());
            foreach (var city in cities)
            {
                city.AddNewGodForFaithfull(City.Invader);
            }
        }
        public void SetVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
        }
        public float GetRange()
        {
            if (_rangeDecorator == null) return _minRange;
            return _rangeDecorator.GetRange();
        }

        private IEnumerator GeneratePriests()
        {
            while (true)
            {
                _city.AddPriests(_city.Invader, _growthOfPriests);
                yield return new WaitForSeconds(_priestsRate);
            }
        }
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!Interactable || City.PriestsAmount == 0) return;

            _dragging = true;
            _signalBus.Fire(new TempleDragBeginSignal { Temple = this });
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!Interactable || !_dragging) return;

            _dragging = false;
            _signalBus.Fire(new TempleDragEndSignal { Temple = this, Target = eventData.pointerEnter?.GetComponent<CityScript>() });
        }

        public bool Equals(TempleStrategy other)
        {
            return _city.Equals(other._city);
        }
    }
}
