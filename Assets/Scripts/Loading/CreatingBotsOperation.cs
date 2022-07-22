using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class CreatingBotsOperation : ILoadingOperation
    {
        public string Description => "Creating bots...";

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            onLoading?.Invoke(0.1f);

            await Task.Delay(TimeSpan.FromSeconds(1f));
            onLoading?.Invoke(0.5f);

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