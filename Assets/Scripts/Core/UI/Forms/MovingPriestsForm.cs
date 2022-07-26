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
        [Header("References")]
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

        private TaskCompletionSource<ushort> _taskCompletionSource = new TaskCompletionSource<ushort>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private void Awake()
        {
            _count.text = _slider.minValue.ToString();
            _go.onClick.AddListener(OnConfirmed);
            _close.onClick.AddListener(OnCancelled);
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
        private void OnConfirmed()
        {
            _taskCompletionSource.SetResult((ushort)_slider.value);
            Close();
        }
        private void OnCancelled()
        {
            _cancellationTokenSource?.Cancel();
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
            if (string.IsNullOrEmpty(label)) return;
            _label.text = label;
        }
        void IConfirmAwaiter<ushort>.SetDescription(string description) { }
        public async Task<ushort> AwaitForConfirm()
        {
            _cancellationTokenSource.Token.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, _cancellationTokenSource.Token);
        }
        public async Task<ushort> AwaitForConfirm(CancellationToken externalToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, externalToken);
            _cancellationTokenSource.Token.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, _cancellationTokenSource.Token);
        }
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}