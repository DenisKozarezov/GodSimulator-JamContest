using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Core.UI.Forms;
using Core.Cities;

namespace Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SacrificeForm : MonoBehaviour, IConfirmAwaiter<bool>, IClosableForm
    {
        [Header("References")]
        [SerializeField]
        private Button _okButton;
        [SerializeField]
        private Button _noButton;
        [SerializeField, Min(0f)]
        private float _offsetY = 1f;

        private RectTransform _rectTransform;
        private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        private void Awake()
        {
            _okButton.onClick.AddListener(OnAccept);
            _noButton.onClick.AddListener(OnDenied);
            _rectTransform = GetComponent<RectTransform>();
        }
        private void OnAccept()
        {
            _taskCompletionSource.SetResult(true);
            Close();
        }
        private void OnDenied()
        {
            _taskCompletionSource.SetResult(false);
            Close();
        }

        public void SetDescription(string description) { }
        public void SetLabel(string label) { }
        public async Task<bool> AwaitForConfirm()
        {
            return await _taskCompletionSource.Task;
        }

        public async Task<bool> AwaitForConfirm(CancellationToken externalToken)
        {
            externalToken.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, externalToken);
        }
        public void AttachToCity(CityScript city)
        {
            _rectTransform.position = Utils.WorldToScreenPoint(city.transform.position + Vector3.up * _offsetY);
        }

        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}