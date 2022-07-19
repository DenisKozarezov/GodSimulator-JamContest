using System.Collections;
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
        [SerializeField, Min(0f)]
        private float _startRange;

        private SignalBus _signalBus;
        private TempleRangeDecorator _rangeDecorator;
        private VirtueModel _virtue;
        private byte _growthOfPriests;
        private Coroutine _generatePriests;
        private CityScript _city;

        private bool _dragging;
        public bool Interactable { get; set; }

        public CityScript City => _city;
        public VirtueModel Virtue => _virtue;
        public float StartRange => _startRange;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Start()
        {
            _city = GetComponent<CityScript>();

            var numberOfCapturedPriests = _city.NumberOfPriests;
            numberOfCapturedPriests.Remove(_city.Invader);
            foreach (var priests in numberOfCapturedPriests)
            {
                _city.NumberOfPriests[priests.Key] = 0;
                _city.AddPriests(_city.Invader, priests.Value);
            }

            _rangeDecorator = new TempleRangeVirtueLevelDecorator(this);
            _growthOfPriests = 1;
            _generatePriests = StartCoroutine(GeneratePriests());
            _city.IncreasePercentageOfFaithfulInOtherCities();
        }

        public void SetVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
        }
        public float GetRange()
        {
            if (_rangeDecorator == null) return _startRange;
            return _rangeDecorator.GetRange();
        }

        private IEnumerator GeneratePriests()
        {
            while (true)
            {
                _city.AddPriests(_city.Invader, _growthOfPriests);
                yield return new WaitForSeconds(10f);
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
    }
}
