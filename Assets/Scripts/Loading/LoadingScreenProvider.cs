using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Loading
{
    public interface ILoadingScreenProvider
    {
        Task LoadAndDestroy(Queue<IAsyncLoadingOperation> loadingOperations);
    }

    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private const string LoadingPrefabPath = "";

        public Task LoadAndDestroy(Queue<IAsyncLoadingOperation> loadingOperations)
        {
            throw new System.NotImplementedException();
        }
    }
}