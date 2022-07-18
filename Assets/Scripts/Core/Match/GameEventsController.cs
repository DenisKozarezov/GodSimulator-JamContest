using System.Linq;
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
        private readonly GameSettings _gameSettings;
        private readonly ILogger Logger;

        public GameEventsController(SignalBus signalBus, MapController mapController, GameSettings gameSettings, ILogger logger)
        {
            _signalBus = signalBus;
            _mapController = mapController;
            _gameSettings = gameSettings;
            Logger = logger;
        }

        private async void OnGameStarted()
        {
            var city = _mapController.Cities.First();
            var form = (SacrificeForm)SacrificeForm.CreateForm(city);

#if UNITY_EDITOR
            form.Elapsed += () => Logger.Log($"Player <b><color=yellow>ignored</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif

            form.StartTimer(_gameSettings.SacrificeDuration);
            bool accepted = await form.AwaitForConfirm();

#if UNITY_EDITOR
            Logger.Log($"Player <b>{(accepted ? "<color=green>accepted" : "<color=red>denied")}</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif
        }
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        }
    }
}
