using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class SceneCleanupOperation : ILoadingOperation
    {
        private readonly ICleanup _cleanup;
        public string Description => "Cleaning the scene...";

        public SceneCleanupOperation(ICleanup cleanupScene)
        {
            _cleanup = cleanupScene;
        }

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            _cleanup.Cleanup();
            onLoading?.Invoke(MathUtils.Random.NextFloat(0.2f, 0.6f));
            await Task.Delay(TimeSpan.FromSeconds(1f));
            onLoading?.Invoke(1f);
        }
        public void Abort()
        {
            throw new NotImplementedException();
        }
        public void Retry()
        {
            throw new NotImplementedException();
        }
        public void Skip()
        {
            throw new NotImplementedException();
        }
    }
}
