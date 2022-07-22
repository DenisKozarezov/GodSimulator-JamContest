using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class WaitingForPlayersOperation : ILoadingOperation
    {
        public string Description => "Waiting for other players...";

        public Task AwaitForLoad(Action<float> onLoading)
        {
            throw new NotImplementedException();
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