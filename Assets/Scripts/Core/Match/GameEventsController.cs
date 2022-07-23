using System;
using System.Threading;
using System.Threading.Tasks;
using Zenject;
using Core.Infrastructure;
using Core.Cities;
using Core.UI;
using static Core.Models.GameSettingsInstaller;
using UnityEngine;

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
            _signalBus.Subscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Subscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
        }
        void ILateDisposable.LateDispose()
        {
            _gameTimerSource?.Cancel();
            _gameTimerSource?.Dispose();
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Unsubscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
        }
        private void OnGameStarted()
        {
            var city = _mapController.Cities.Randomly();
            OfferSacrificeInCity(city);
        }
        private void OnPlayerCastedAbility(PlayerCastedTargetAbilitySignal signal)
        {
            var prefab = Resources.Load(signal.Ability.EffectPrefab) as GameObject;

            if (prefab == null)
            {
#if UNITY_EDITOR
                _logger.Log($"Unable to load <b><color=yellow>effect prefab</color></b> for <b><color=yellow>{signal.Ability.DisplayName}</color></b> ability!", LogType.Critical);
#endif
                return;
            }
            var effect = GameObject.Instantiate(prefab, signal.Target.transform.position, Quaternion.identity);
        }
        
        private async void SetApocalypseTimer()
        {
            try
            {
                _gameTimerSource = new CancellationTokenSource();
                await Task.Delay(TimeSpan.FromSeconds(_gameSettings.GameTime), _gameTimerSource.Token);
                StartApocalypse();
            }
            catch (TaskCanceledException e)
            {
#if UNITY_EDITOR
                _logger.Log("<b><color=yellow>Game Apocalypse</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
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
        private void StartApocalypse()
        {
            _signalBus.Fire<GameApocalypseSignal>();
        }
    }
}
