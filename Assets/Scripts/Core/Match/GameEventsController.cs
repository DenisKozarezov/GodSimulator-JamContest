using Zenject;
using Core.Infrastructure;
using Core.UI;
using static Core.Models.GameSettingsInstaller;

namespace Core.Match
{
    public class GameEventsController : IInitializable, ILateDisposable
    {        
        private readonly SignalBus _signalBus;
        private readonly MapController _mapController;
        private readonly SacrificeSettings _sacrificeSettings;
        private readonly ILogger _logger;

        public GameEventsController(SignalBus signalBus, MapController mapController, SacrificeSettings gameSettings, ILogger logger)
        {
            _signalBus = signalBus;
            _mapController = mapController;
            _sacrificeSettings = gameSettings;
            _logger = logger;
        }
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        }
        private async void OnGameStarted()
        {
            var city = _mapController.Cities.Randomly();
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
    }
}
