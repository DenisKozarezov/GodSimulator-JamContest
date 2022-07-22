using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Loading
{
    public class GameLoadingOperation : ILoadingOperation
    {
        public string Description => "Loading game...";

        public GameLoadingOperation()
        {

        }

        public async Task<float> AwaitForLoad(Action onLoad)
        {
            var operation = Resources.LoadAsync(Constants.GameScene);
 
            await Task.Run(() =>
            {
                do return operation.progress;
                while (!operation.isDone);
            });

            return await Task.FromResult(1f);
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