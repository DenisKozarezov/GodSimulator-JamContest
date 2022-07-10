using Core.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Forms
{
    public class MovingPriestsPanel : MonoBehaviour, IClosableForm
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _count;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Button _go;
        [SerializeField]
        private Button _close;
        private GreeceCityScript _city;

        private SignalBus _signalBus;
        protected SignalBus SignalBus => _signalBus;

        [Inject]
        public void Contruct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _go.onClick.AddListener(ActivateSelectionMode);
            _close.onClick.AddListener(Close);
        }

        public void InitializePanel(PlayerClickedOnCitySignal playerClickedOnCitySignal)
        {
            InitializeSlider(playerClickedOnCitySignal.NumberOfPriests);
            _city = playerClickedOnCitySignal.City;
        }

        private void InitializeSlider(ushort maxNumberOfPriests)
        {
            _slider.maxValue = maxNumberOfPriests;
        }

        public void SliderChanged()
        {
            _count.text = _slider.value.ToString();
        }

        public void ActivateSelectionMode()
        {
            if (_city != null)
            {
                SignalBus.Fire(new SelectionModeChangedSignal { Value = true });
                _city.ShowRangeToCities();
                Close();
            }
        }

        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}