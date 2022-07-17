using System.Threading;
using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IConfirmAwaiter
    {
        void SetLabel(string label);
        void SetDescription(string description);
        Task AwaitForConfirm();
        Task AwaitForConfirm(CancellationToken externalToken);
    }
    public interface IConfirmAwaiter<TResult>
    {
        void SetLabel(string label);
        void SetDescription(string description);
        Task<TResult> AwaitForConfirm();
        Task<TResult> AwaitForConfirm(CancellationToken externalToken);
    }
}