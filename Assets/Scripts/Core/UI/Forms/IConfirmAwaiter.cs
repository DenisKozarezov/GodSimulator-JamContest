using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IConfirmAwaiter
    {
        void SetLabel(string label);
        void SetDescription(string description);
        Task AwaitForConfirm();
    }
}