using System.Threading;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using System.Threading.Tasks;
using static Core.Models.GameSettingsInstaller;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        private SignalBus _signalBus;
        private GameSettings _gameSettings;

        private CancellationTokenSource _gameTimerSource;

        [Inject]
        public void Construct(SignalBus signalBus, GameSettings settings)
        {
            _signalBus = signalBus;
            _gameSettings = settings;
        }

        private void Start()
        {
            StartGame();
        }
        private void OnDestroy()
        {
            _gameTimerSource?.Cancel();
            _gameTimerSource?.Dispose();
        }

        public async void StartGame()
        {
            _signalBus.Fire<GameStartedSignal>();
            _gameTimerSource = new CancellationTokenSource();
            await Task.Delay(_gameSettings.GameTime * 1000, _gameTimerSource.Token);
            _signalBus.Fire<GameApocalypsisSignal>();
        }
    }
}