using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Core.Models;
using Core.Infrastructure;
using Zenject;
using System;

namespace Core.Cities
{
    public class TempleStrategy : MonoBehaviour, ICityStrategy,
        IBeginDragHandler, IEndDragHandler
    {
        private CityScript _city;
        private float _range;

        private SignalBus _signalBus;
        private VirtueModel _virtue;
        private byte _growthOfPriests;
        private Coroutine _generatePriests;

        public bool Interactable { get; set; }

        public CityScript City => _city;
        public VirtueModel Virtue => _virtue;
        public float Range => _range;

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

            _range = GetRange(1);
            _growthOfPriests = 1;
            _generatePriests = StartCoroutine(GeneratePriests());
            _city.IncreasePercentageOfFaithfulInOtherCities();
        }

        private float GetRange(byte virtueLevel)
        {
            switch (virtueLevel)
            {
                case 1:
                    return 3f;
                case 2:
                    return 5f;
                case 3:
                    return 10f;
            }
            return 0;
        }
        public void SetVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
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
            if (!Interactable) return;

            _signalBus.Fire(new TempleDragBeginSignal { Temple = this });
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!Interactable) return;

            _signalBus.Fire(new TempleDragEndSignal { God = _city.Invader, Temple = this, Target = eventData.pointerEnter?.GetComponent<CityScript>() });
        }
    }
}
