using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class DecisionForm : MonoBehaviour, IConfirmAwaiter<bool>, IAutoSizable, IClosableForm
    {
        private const string FormPath = "Prefabs/Views/Decision Form";

        [Header("References")]
        [SerializeField]
        private TMP_Text _label;
        [SerializeField]
        private TMP_Text _description;
        [SerializeField]
        private Button _okButton;
        [SerializeField]
        private Button _denyButton;

        [Header("Form")]
        [SerializeField]
        private bool _autoSize;
        [SerializeField, Min(0f)]
        private float _minHeight;

        private RectTransform _rectTransform;
        private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public bool AutoSize => _autoSize;
        public float MinHeight => _minHeight;

        private void Awake()
        {
            _okButton.onClick.AddListener(OnAccept);
            _denyButton.onClick.AddListener(OnDenied);
            _rectTransform = GetComponent<RectTransform>();
        }
        private void Start()
        {
            if (AutoSize)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, MinHeight + _description.preferredHeight);
            }
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
        public void SetLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return;
            _label.text = label;
        }
        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description)) return;
            _description.text = description;
        }
        public static IConfirmAwaiter<bool> CreateForm(string label = null, string description = null)
        {
            var obj = Instantiate(Resources.Load(FormPath)) as GameObject;
            var form = obj.GetComponentInChildren<IConfirmAwaiter<bool>>();
            form.SetLabel(label);
            form.SetDescription(description);
            return form;
        }
        public async Task<bool> AwaitForConfirm()
        {
            return await _taskCompletionSource.Task;
        }
        public async Task<bool> AwaitForConfirm(CancellationToken externalToken)
        {
            externalToken.Register(Close);
            return await Task.Run(() => _taskCompletionSource.Task, externalToken);
        }
        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}