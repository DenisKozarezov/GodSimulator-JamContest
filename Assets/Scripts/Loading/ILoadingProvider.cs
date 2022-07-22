using System.Collections.Generic;

namespace Core.Loading
{
    public interface ILoadingProvider
    {
        void LoadAndDestroy(Queue<ILoadingOperation> operations);
    }
}