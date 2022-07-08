using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Core.Infrastructure;

namespace Core.UI
{
    public class GodParametersView : MonoBehaviour
    {
        [SerializeField]
        private Slider _warSlider;
        [SerializeField]
        private Slider _natureSlider;
        [SerializeField]
        private Slider _loveSlider;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<GodParametersChangedSignal>(OnParametersChanged);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<GodParametersChangedSignal>(OnParametersChanged);
        }

        private void OnParametersChanged(GodParametersChangedSignal param)
        {
            _warSlider.value = param.War;
            _natureSlider.value = param.Nature;
            _loveSlider.value = param.Love;
        }
    }
}