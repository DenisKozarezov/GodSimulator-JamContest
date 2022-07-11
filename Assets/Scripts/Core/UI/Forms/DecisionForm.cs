using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace Core.UI.Forms
{
    [RequireComponent(typeof(RectTransform))]
    public class DecisionForm : MonoBehaviour, IDecisionAwaiter, IAutoSizable, IClosableForm
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
        private TaskCompletionSource<bool> _taskCompletion;

        public bool AutoSize => _autoSize;
        public float MinHeight => _minHeight;

        private void Awake()
        {
            _okButton.onClick.AddListener(OnAccept);
            _denyButton.onClick.AddListener(OnDenied);
            _rectTransform = GetComponent<RectTransform>();
        }
        private void OnAccept()
        {
            _taskCompletion.SetResult(true);
        }
        private void OnDenied()
        {
            _taskCompletion.SetResult(false);
        }
        public static IDecisionAwaiter CreateForm(string label = null, string description = null)
        {
            var obj = Instantiate(Resources.Load(FormPath)) as GameObject;
            var form = obj.GetComponentInChildren<IDecisionAwaiter>();
            form.SetLabel(label);
            form.SetDescription(description);
            return form;
        }
        public async Task<bool> AwaitForDecision()
        {
            if (AutoSize)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, MinHeight + _description.preferredHeight);
            }

            _taskCompletion = new TaskCompletionSource<bool>();
            bool result = await _taskCompletion.Task;
            Close();
            return result;
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
        public void Close()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}