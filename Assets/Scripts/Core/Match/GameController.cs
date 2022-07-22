using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using static Core.Models.GameSettingsInstaller;

namespace Core.Match
{
    public class GameController : MonoBehaviour
    {
        private ILogger _logger;
        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private CancellationTokenSource _gameTimerSource;

        [Inject]
        private void Construct(SignalBus signalBus, GameSettings settings, ILogger logger)
        {
            _signalBus = signalBus;
            _gameSettings = settings;
            _logger = logger;
        }

        private void Awake()
        {
            SceneController.SceneLoaded += (scene) => _signalBus.Fire(new SceneLoadedSignal { Scene = scene });
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

            try
            {
                _gameTimerSource = new CancellationTokenSource();
                await Task.Delay(TimeSpan.FromSeconds(_gameSettings.GameTime), _gameTimerSource.Token);
                _signalBus.Fire<GameApocalypsisSignal>();
            }
            catch (TaskCanceledException e)
            {
#if UNITY_EDITOR
                _logger.Log("<b><color=yellow>Game Apocalypsis</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
#endif
            }
        }
    }
}