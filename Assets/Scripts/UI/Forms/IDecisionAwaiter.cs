using System.Threading.Tasks;

namespace Core.UI.Forms
{
    public interface IDecisionAwaiter
    {
        Task<bool> AwaitForDecision();
    }
}