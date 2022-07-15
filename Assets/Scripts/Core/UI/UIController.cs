using System;
using System.Threading;
using UnityEngine;
using Zenject;
using DG.Tweening;
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
        [Serializable]
        struct Cursors
        {
            public Texture2D _targetCursor;
        }

        [SerializeField]
        private GameObject _movingPriestsForm;
        [SerializeField]
        private GameObject _animatedDottedLine;
        [SerializeField]
        private GameObject _movingPriestsIcon;
        [SerializeField]
        private SerializableDictionaryBase<CursorType, Texture2D> _cursors;
        private bool _selectionMode;

        private SignalBus _signalBus;
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

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

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private void OnTempleDragBegin(TempleDragBeginSignal signal)
        {
            SetCursor(CursorType.Target);
            if (signal.View != null)
            {
                SetSelectionMode(true);            
                signal.View.ShowRangeToCities();
            }
        }
        private void OnTempleDragEnd(TempleDragEndSignal signal)
        {
            SetCursor(CursorType.Default);
            SetSelectionMode(false);            
        }
        private async void OnPlayerSelectedCityForPriests(TempleDragEndSignal signal)
        {
            if (signal.View.Equals(signal.Target)) return;

            var form = Instantiate(_movingPriestsForm).GetComponent<MovingPriestsForm>();
            form.Init(signal.Target.NumberOfPriests);

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            form.Cancelled += () => _cancellationTokenSource.Cancel();

            ushort priestsCount = await form.AwaitForConfirm(_cancellationTokenSource.Token);

            // Create Dotted Line and Icon
            if (signal.Target == null) return;

            Vector2 startPos = signal.View.transform.position;
            Vector2 endPos = signal.Target.transform.position;

            CreateAnimatedTransition(startPos, endPos, 10f);
        }

        private void CreateAnimatedTransition(Vector2 startPos, Vector2 endPos, float time)
        {
            var line = Instantiate(_animatedDottedLine).GetComponent<AnimatedDottedLine>();
            line.StartPosition = startPos;
            line.EndPosition = endPos;

            var icon = Instantiate(_movingPriestsIcon, startPos, Quaternion.identity);
            icon.transform.DOMove(endPos, time).OnComplete(() =>
            {
                Destroy(icon.gameObject);
                Destroy(line.gameObject);
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