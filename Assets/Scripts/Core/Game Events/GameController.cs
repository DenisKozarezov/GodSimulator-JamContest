using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using static Core.Models.GameSettingsInstaller;
using System.Collections.Generic;
using System.Linq;
using Core.Cities;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        private ILogger Logger;
        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private CancellationTokenSource _gameTimerSource;

        [Inject]
        public void Construct(SignalBus signalBus, GameSettings settings, ILogger logger)
        {
            _signalBus = signalBus;
            _gameSettings = settings;
            Logger = logger;
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
                Logger.Log("<b><color=yellow>Game Apocalypsis</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
            }
        }
    }
}