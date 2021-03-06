using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Core.Models;

namespace Core.UI.Forms
{
    public class ChooseForm : MonoBehaviour, IConfirmAwaiter<VirtueModel>, IClosableForm
    {
        [SerializeField]
        private Button _readyButton;

        private TaskCompletionSource<VirtueModel> _taskCompletionSource = new TaskCompletionSource<VirtueModel>();

        private void Awake()
        {
            _readyButton.onClick.AddListener(OnReady);
        }
        private void OnReady()
        {
            _readyButton.interactable = false;
            _taskCompletionSource.SetResult(null);
            Close();
        }

        void IConfirmAwaiter<VirtueModel>.SetDescription(string description) { }
        void IConfirmAwaiter<VirtueModel>.SetLabel(string label) { }
        public async Task<VirtueModel> AwaitForConfirm()
        {
            return await _taskCompletionSource.Task;
        }
        public async Task<VirtueModel> AwaitForConfirm(CancellationToken externalToken)
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