using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class MovingPriestsForm : MonoBehaviour, IConfirmAwaiter<ushort>, IClosableForm
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

        public event Action Cancelled;

        private void Awake()
        {
            _count.text = _slider.minValue.ToString();
            _go.onClick.AddListener(OnConfirmed);
            _close.onClick.AddListener(OnCancelled);
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }
        private void OnConfirmed()
        {          
            _taskCompletionSource.SetResult((ushort)_slider.value);
            Close();
        }
        private void OnCancelled()
        {
            Cancelled?.Invoke();
            Close();
        }
        private void OnSliderChanged(float value)
        {
            _count.text = value.ToString();
        }

        public void Init(float sliderMaxLimit)
        {
            _slider.maxValue = sliderMaxLimit;
        }
        public void SetLabel(string label)
        {
            _label.text = label;
        }
        public void SetDescription(string description) { }
        public async Task<ushort> AwaitForConfirm()
        {
            _taskCompletionSource = new TaskCompletionSource<ushort>();
            return await _taskCompletionSource.Task;
        }
        public async Task<ushort> AwaitForConfirm(CancellationToken cancellationToken)
        {
            _taskCompletionSource = new TaskCompletionSource<ushort>();
            return await Task.Run(() => _taskCompletionSource.Task, cancellationToken);
        }
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}