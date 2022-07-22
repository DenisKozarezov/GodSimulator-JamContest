using System;
using System.Threading.Tasks;

namespace Core.Loading
{
    public class CreatingBotsOperation : ILoadingOperation
    {
        public string Description => "Creating bots...";

        public CreatingBotsOperation()
        {

        }

        public async Task<float> AwaitForLoad(Action onLoad)
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