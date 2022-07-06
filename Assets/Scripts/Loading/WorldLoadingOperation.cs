using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class WorldLoadingOperation : IAsyncLoadingOperation
    {
        public string Description => "Loading the world...";

        public Task Load(Action<float> onProgress)
        {
            throw new NotImplementedException();
        }
    }
}