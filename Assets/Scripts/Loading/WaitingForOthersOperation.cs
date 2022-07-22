using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class WaitingForOthersOperation : ILoadingOperation
    {
        public string Description => "Waiting for others...";

        public WaitingForOthersOperation()
        {

        }

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