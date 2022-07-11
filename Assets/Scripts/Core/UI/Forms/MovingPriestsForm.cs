using Core.Cities;
using Core.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
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
        private GreeceCityScript _view;

        private SignalBus _signalBus;

        public event Action Agreed;

        [Inject]
        public void Contruct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _go.onClick.AddListener(ActivateSelectionMode);
            _close.onClick.AddListener(Close);
        }

        public void Init(PlayerClickedOnCitySignal playerClickedOnCitySignal)
        {
            InitializeSlider(playerClickedOnCitySignal.NumberOfPriests);
            _view = playerClickedOnCitySignal.View;
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
            if (_view != null)
            {
                _signalBus.Fire(new SelectionModeChangedSignal { Value = true });
                _view.ShowRangeToCities();
                Close();
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}