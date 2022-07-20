using UnityEngine;
using Unity.Mathematics;
using Zenject;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.UI.Forms;

namespace Core.UI
{ 
    public enum CursorType : byte
    {
        Default = 0x00,
        Target = 0x01,
    }

    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionaryBase<CursorType, Texture2D> _cursors;
        private bool _selectionMode;

        private const string FormPrefab = "Prefabs/Views/Forms/Moving Priests Form";

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Awake()
        {
            _signalBus.Subscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Subscribe<TempleDragEndSignal>(OnTempleDragEnd);
            _signalBus.Subscribe<TempleDragEndSignal>(OnPlayerSelectedCityForPriests);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<TempleDragBeginSignal>(OnTempleDragBegin);
            _signalBus.Unsubscribe<TempleDragEndSignal>(OnTempleDragEnd);
            _signalBus.Unsubscribe<TempleDragEndSignal>(OnPlayerSelectedCityForPriests);
        }

        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            if (signal.Temple != null)
            {
                SetCursor(CursorType.Target);
                SetSelectionMode(true);
            }
        }
        private void OnTempleDragEnd(TempleDragEndSignal signal)
        {
            SetCursor(CursorType.Default);
            SetSelectionMode(false);            
        }
        private async void OnPlayerSelectedCityForPriests(TempleDragEndSignal signal)
        {
            if (signal.Target == null || signal.Temple.Equals(signal.Target)) return;

            float sqrDistance = math.distancesq(signal.Temple.transform.position, signal.Target.transform.position);
            float sqrRange = math.pow(signal.Temple.GetRange(), 2);
            if (sqrDistance >= sqrRange) return;

            var asset = Resources.Load<MovingPriestsForm>(FormPrefab);
            var form = Instantiate(asset);
            form.Init(signal.Temple.City.PriestsAmount);

            ushort priestsCount = await form.AwaitForConfirm();

            _signalBus.Fire(new PlayerMovingPriestsSignal
            {
                Temple = signal.Temple,
                Target = signal.Target,
                Duration = 10f,
                PriestsAmount = priestsCount
            });
        }

        private void SetCursor(CursorType cursorType)
        {
            Cursor.SetCursor(_cursors[cursorType], Vector2.zero, CursorMode.Auto);
        }
        private void SetSelectionMode(bool isSelected)
        {
            _selectionMode = isSelected;
        }
    }
}