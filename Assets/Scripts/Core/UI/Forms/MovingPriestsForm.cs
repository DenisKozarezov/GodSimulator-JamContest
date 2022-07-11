using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Core.Infrastructure;
using Core.Cities;
using Zenject;
using TMPro;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class MovingPriestsForm : MonoBehaviour, IClosableForm, IConfirmAwaiter<ushort>
    {
        [SerializeField]
        private TextMeshProUGUI _label;
        [SerializeField]
        private TextMeshProUGUI _count;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Button _go;
        [SerializeField]
        private Button _close;

        private TaskCompletionSource<ushort> _taskCompletionSource;

        private void Awake()
        {
            _go.onClick.AddListener(OnConfirmed);
            _close.onClick.AddListener(Close);
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }
        private void OnConfirmed()
        {          
            _taskCompletionSource.SetResult((ushort)_slider.value);
        }
        private void OnSliderChanged(float value)
        {
            _count.text = value.ToString();
        }

        public void Init(float sliderMaxLimit)
        {
            _slider.maxValue = sliderMaxLimit;
        }

        public async Task<ushort> AwaitForConfirm()
        {
            _taskCompletionSource = new TaskCompletionSource<ushort>();
            var result = await _taskCompletionSource.Task;
            Close();
            return result;
        }
        public void SetLabel(string label)
        {
            _label.text = label;
        }
        public void SetDescription(string description) { }   
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}