using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Loading
{
    public interface ILoadingProvider
    {
        Task LoadAndDestroy(Queue<LazyLoadingOperation> operations);
    }
}