using System.Threading;
using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IConfirmAwaiter<ResultT>
    {
        void SetLabel(string label);
        void SetDescription(string description);
        Task<ResultT> AwaitForConfirm();
        Task<ResultT> AwaitForConfirm(CancellationToken cancelletionToken);
    }
}