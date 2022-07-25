using System;
using System.Threading.Tasks;
using Core.Match;
using Zenject;

namespace Core.Loading
{
    public class CreatingBotsOperation : ILoadingOperation
    {
        private GameController _controller;

        public byte Count;
        public string Description => "Creating bots...";

        [Inject]
        private void Construct(GameController controller)
        {
            _controller = controller;
        }

        public async Task AwaitForLoad(Action<float> onLoading)
        {
            var controller = _controller; 
            onLoading?.Invoke(MathUtils.Random.NextFloat(0.1f, 0.4f));

            await Task.Delay(TimeSpan.FromSeconds(1f));
            onLoading?.Invoke(MathUtils.Random.NextFloat(0.4f, 0.8f));

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