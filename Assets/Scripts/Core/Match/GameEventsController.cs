using System;
using System.Threading;
using System.Threading.Tasks;
using Zenject;
using Core.Infrastructure;
using Core.Cities;
using Core.UI;
using static Core.Models.GameSettingsInstaller;

namespace Core.Match
{
    public class GameEventsController : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MapController _mapController;
        private readonly SacrificeSettings _sacrificeSettings;
        private readonly GameSettings _gameSettings;
        private readonly ILogger _logger;

        private CancellationTokenSource _gameTimerSource;

        public GameEventsController(
            SignalBus signalBus,
            MapController mapController,
            SacrificeSettings sacrificeSettings,
            GameSettings gameSettings,
            ILogger logger)
        {
            _signalBus = signalBus;
            _mapController = mapController;
            _sacrificeSettings = sacrificeSettings;
            _gameSettings = gameSettings;
            _logger = logger;
        }
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<GameStartedSignal>(SetApocalypsisTimer);
        }
        void ILateDisposable.LateDispose()
        {
            _gameTimerSource?.Cancel();
            _gameTimerSource?.Dispose();
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<GameStartedSignal>(SetApocalypsisTimer);
        }
        private void OnGameStarted()
        {
            var city = _mapController.Cities.Randomly();
            OfferSacrificeInCity(city);
        }
        private async void SetApocalypsisTimer()
        {
            try
            {
                _gameTimerSource = new CancellationTokenSource();
                await Task.Delay(TimeSpan.FromSeconds(_gameSettings.GameTime), _gameTimerSource.Token);
                StartApocalypsis();
            }
            catch (TaskCanceledException e)
            {
#if UNITY_EDITOR
                _logger.Log("<b><color=yellow>Game Apocalypsis</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
#endif
            }
        }
        private async void OfferSacrificeInCity(CityScript city)
        {
            var form = (SacrificeForm)SacrificeForm.CreateForm(city);

#if UNITY_EDITOR
            form.Elapsed += () => _logger.Log($"Player <b><color=yellow>ignored</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif

            form.StartTimer(_sacrificeSettings.Duration);
            bool accepted = await form.AwaitForConfirm();

#if UNITY_EDITOR
            _logger.Log($"Player <b>{(accepted ? "<color=green>accepted" : "<color=red>denied")}</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif
        }
        private void StartApocalypsis()
        {
            _signalBus.Fire<GameApocalypsisSignal>();
        }
    }
}
