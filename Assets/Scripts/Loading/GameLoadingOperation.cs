using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        public string Description => "Loading game...";

        public GameLoadingOperation()
        {

        }

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            var operation = SceneManager.LoadSceneAsync(Constants.GameScene, LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                onLoading?.Invoke(operation.progress);
                await Task.Yield();
            }

            await Task.Delay(TimeSpan.FromSeconds(1.5f));
            onLoading?.Invoke(0.5f);

            operation = SceneManager.UnloadSceneAsync(Constants.MainMenu);
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