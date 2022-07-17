using Core.Cities;
using Core.Infrastructure;
using Core.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Forms
{
    public class MovingPriestsForm : MonoBehaviour, IClosableForm
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _count;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Button _go;
        [SerializeField]
        private Button _close;
        private GodModel _god;
        private GreeceCityScript _fromCity;
        private GreeceCityScript _toCity;

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
            _god = playerClickedOnCitySignal.God;
            _fromCity = playerClickedOnCitySignal.FromCity;
            _toCity = playerClickedOnCitySignal.ToCity;
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
            if (_fromCity != null)
            {
                SignalBus.Fire(new SelectionModeChangedSignal { Value = true });
                SignalBus.Fire(new PlayerMovingPriestsSignal { God = _god, ToCity = _toCity, FromCity = _fromCity, NumberOfPriests = (byte)_slider.value });
                Close();
            }
        }

        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}