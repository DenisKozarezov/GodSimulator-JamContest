using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public interface ILoadingOperation
    {
        string Description { get; }
        Task AwaitForLoad(Action<float> onLoading);
        void Abort();
        void Retry();
        void Skip();
    }
}