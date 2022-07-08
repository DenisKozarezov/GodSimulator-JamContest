using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IDecisionAwaiter
    {
        Task<bool> AwaitForDecision();
        void SetLabel(string label);
        void SetDescription(string description);
    }
}