using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IConfirmAwaiter<ResultT>
    {
        void SetLabel(string label);
        void SetDescription(string description);
        Task<ResultT> AwaitForConfirm();
    }
}