using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Core.UI.Forms;
using Core.Cities;
using Core.Models;
using DG.Tweening;

namespace Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SacrificeForm : MonoBehaviour, IConfirmAwaiter<bool>, IClosableForm
    {
        private const string FormPath = "Prefabs/UI/Forms/Sacrifice Form";

        [Header("References")]
        [SerializeField]
        private Button _okButton;
        [SerializeField]
        private Button _noButton;
        [SerializeField]
        private PranaUIView _pranaView;
        [SerializeField]
        private Image _icon;
        [SerializeField, Min(0f)]
        private float _offsetY = 1f;

        private CityScript _attachedCity;
        private RectTransform _rectTransform;
        private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event Action Elapsed;

        private void SetModel(SacrificeModel model)
        {
            _icon.sprite = model.Icon;
        }
        private void Awake()
        {
            _okButton.onClick.AddListener(OnAccept);
            _noButton.onClick.AddListener(OnDenied);
            _rectTransform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            if (_attachedCity == null) return;
            _rectTransform.position = Utils.WorldToScreenPoint(_attachedCity.transform.position + Vector3.up * _offsetY);
        }
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
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

        void IConfirmAwaiter<bool>.SetDescription(string description) { }
        void IConfirmAwaiter<bool>.SetLabel(string label) { }
        public static IConfirmAwaiter<bool> CreateForm(SacrificeModel sacrifice, CityScript target)
        {
            var obj = Instantiate(Resources.Load(FormPath)) as GameObject;
            var form = obj.GetComponentInChildren<SacrificeForm>();
            form.SetModel(sacrifice);
            form._attachedCity = target;
            return form;
        }
        public async Task<bool> AwaitForConfirm()
        {
            _cancellationTokenSource.Token.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, _cancellationTokenSource.Token);
        }
        public async Task<bool> AwaitForConfirm(CancellationToken externalToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, externalToken);
            _cancellationTokenSource.Token.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, _cancellationTokenSource.Token);
        }
        public void SetInteractable(bool isInteractable)
        {
            _okButton.interactable = isInteractable;
            _noButton.interactable = isInteractable;
        }
        public void StartTimer(float duration)
        {
            DOTween.To(() => 0f, (x) =>
            {
                _pranaView.SetFillAmount(x);
                _pranaView.color = Color.Lerp(Color.white, Color.black, x);
            }, 1f, duration)
            .SetEase(Ease.Linear)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                _cancellationTokenSource?.Cancel();
                Elapsed?.Invoke();
            });
        }
        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}