using System.Threading;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using Core.UI.Forms;

namespace Core.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _movingPriestsForm;
        private bool _selectionMode;

        private SignalBus _signalBus;
        private CancellationTokenSource _cancellationTokenSource;

        public bool SelectionMode => _selectionMode;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<PlayerClickedOnCitySignal>(OnPlayerClickOnCity);
        }
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async void OnPlayerClickOnCity(PlayerClickedOnCitySignal signal)
        {
            var form = Instantiate(_movingPriestsForm).GetComponent<MovingPriestsForm>();
            form.Init(signal.NumberOfPriests);

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            form.Cancelled += () => _cancellationTokenSource.Cancel();

            ushort priestsCount = await form.AwaitForConfirm(_cancellationTokenSource.Token);

            if (signal.View != null)
            {
                SetSelectionMode(true);
                signal.View.ShowRangeToCities();
            }
        }

        public void SetSelectionMode(bool isSelected)
        {
            _selectionMode = isSelected;
        }
    }
}