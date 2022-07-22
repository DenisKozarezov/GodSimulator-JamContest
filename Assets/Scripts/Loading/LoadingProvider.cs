using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Loading
{
    public class LoadingProvider : ILoadingProvider
    {
        private readonly ILogger _logger;

        public LoadingProvider(ILogger logger) => _logger = logger;

        private async Task<LoadingScreen> LoadScreen()
        {
            var load = Resources.LoadAsync<GameObject>("Prefabs/UI/Loading/Loading Screen");
            while (!load.isDone)
            {
                await Task.Yield();
            }

            var obj = GameObject.Instantiate(load.asset as GameObject);
            GameObject.DontDestroyOnLoad(obj);
            return obj.GetComponentInChildren<LoadingScreen>();
        }
        public async Task LoadAndDestroy(Queue<ILoadingOperation> operations)
        {
            var loadingScreen = await LoadScreen();

#if UNITY_EDITOR
            _logger.Log("<b><color=green>[LOADING]</color></b>: Initiating loading process...");
#endif

            await loadingScreen.LoadAndDestroyAsync(operations);

#if UNITY_EDITOR
            _logger.Log("<b><color=green>[LOADING]</color></b>: Loading process has <b><color=yellow>successfully</color></b> finished.");
#endif
        }
    }
}