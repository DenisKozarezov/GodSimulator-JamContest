using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public interface ILoadingOperation
    {
        string Description { get; }
        Task<float> AwaitForLoad(Action onLoad);
        void Abort();
        void Retry();
        void Skip();
    }
}