using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Core.Models;
using Core.Infrastructure;
using Zenject;

namespace Core.Cities
{
    public class TempleStrategy : MonoBehaviour, ICityStrategy,
        IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private byte _maxCapacityOfPriests;
        [SerializeField]
        private float _range = 5f;

        private SignalBus _signalBus;
        private VirtueModel _virtue;
        private byte _growthOfPriests;
        private ushort _numberOfPriests;
        private Coroutine _generatePriests;

        public bool Interactable { get; set; }

        public VirtueModel Virtue => _virtue;
        public byte MaxCapacityOfPriests => _maxCapacityOfPriests;
        public ushort NumberOfPriests => _numberOfPriests;
        public float Range => _range;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Start()
        {
            _growthOfPriests = 1;
            _generatePriests = StartCoroutine(GeneratePriests());
        }

        public void SetVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
        }
        public void AddPriests(ushort value)
        {
            _numberOfPriests += value;
        }
        public void ReducePriests(ushort value)
        {
            _numberOfPriests -= value;
        }
        private IEnumerator GeneratePriests()
        {
            while (true)
            {
                _numberOfPriests += _growthOfPriests;
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

            _signalBus.Fire(new TempleDragEndSignal { Temple = this, Target = eventData.pointerEnter?.GetComponent<CityScript>() });
        }
    }
}
