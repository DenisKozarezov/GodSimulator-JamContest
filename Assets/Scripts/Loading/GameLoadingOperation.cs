using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        private readonly ICleanup _cleanup;
        public string Description => "Loading game...";

        public GameLoadingOperation(ICleanup cleanupScene)
        {
            _cleanup = cleanupScene;
        }

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            var operation = SceneManager.LoadSceneAsync(Constants.Scenes.GameScene, LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                onLoading?.Invoke(operation.progress);
                await Task.Yield();
            }

            await Task.Delay(TimeSpan.FromSeconds(1.5f));
            onLoading?.Invoke(0.5f);

            operation = SceneManager.UnloadSceneAsync(_cleanup.SceneName);
            while (!operation.isDone)
            {
                onLoading?.Invoke(operation.progress);
                await Task.Yield();
            }
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