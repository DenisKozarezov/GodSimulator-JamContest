using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public interface IAsyncLoadingOperation
    {
        string Description { get; }
        Task Load(Action<float> onProgress);
    }
}