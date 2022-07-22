using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class PressAnyButtonOperation : ILoadingOperation
    {
        public string Description => "Press any button...";

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            while (!UnityEngine.Input.anyKeyDown)
            {
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