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

        public Task<float> AwaitForLoad(Action onLoad)
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