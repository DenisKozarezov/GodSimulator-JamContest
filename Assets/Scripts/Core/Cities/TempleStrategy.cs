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
        IBeginDragHandler, IEndDragHandler
    {
        [Header("Temple")]
        [SerializeField, Min(0f)]
        private float _minRange = 3f;
        [SerializeField, Min(0f)]
        private float _priestsRate = 3f;
        [SerializeField]
        private byte _growthOfPriests = 1;

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

        public void Construct(SignalBus signalBus, MapController mapController)
        {
            _signalBus = signalBus;
            _mapController = mapController;
        }

        private void Start()
        {
            if (!GetComponent<EventTrigger>())
            {
                var trigger = gameObject.AddComponent<EventTrigger>();
                trigger.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag });
                trigger.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.EndDrag });
            }

            _city = GetComponent<CityScript>();
            _rangeDecorator = new TempleRangeVirtueLevelDecorator(this);

            _generatePriests = StartCoroutine(GeneratePriests());
            IncreasePercentageOfFaithfulInOtherCities();
        }
        void ICityStrategy.Disable()
        {
            if (_generatePriests != null) StopCoroutine(_generatePriests);
        }

        private void IncreasePercentageOfFaithfulInOtherCities()
        {
            IEnumerable<NeutralStrategy> cities = _mapController.Cities
                .SelectMany<NeutralStrategy>(city => city != this)
                .ByDistance(transform.position, GetRange());
            foreach (var city in cities)
            {
                city.AddNewGodForFaithfull(City.Owner);
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
                yield return new WaitForSeconds(_priestsRate);
                _city.AddPriests(_growthOfPriests);
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

        public bool Equals(ICityStrategy other)
        {
            if (other == null) return false;
            return _city.Equals(other.City);
        }
    }
}
