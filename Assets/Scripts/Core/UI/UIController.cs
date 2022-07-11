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

        private async void OnPlayerClickOnCity(PlayerClickedOnCitySignal signal)
        {
            var form = Instantiate(_movingPriestsForm).GetComponent<MovingPriestsForm>();
            form.Init(signal.NumberOfPriests);

            ushort priestsCount = await form.AwaitForConfirm();

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