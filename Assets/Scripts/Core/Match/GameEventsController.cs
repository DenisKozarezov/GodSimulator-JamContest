using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using Core.Cities;
using Core.UI;
using Core.Models;

namespace Core.Match
{
    public class GameEventsController : MonoBehaviour
    {
        private SignalBus _signalBus;
        private MapController _mapController;
        private SacrificeSettings _sacrificeSettings;
        private GameSettings _gameSettings;
        private ILogger _logger;

        private Lazy<SacrificeModel[]> _sacrifices;

        private CancellationTokenSource _gameTimerSource = new CancellationTokenSource();
        private CancellationTokenSource _sacrificeSource = new CancellationTokenSource();

        [Inject]
        private void Construct(
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
        private void Awake()
        {
            _sacrifices = Utils.CreateLazyArray<SacrificeModel>("Scriptable Objects/Sacrifices");
        }
        private void Start()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Subscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
        }
        private void OnDestroy()
        {
            _gameTimerSource?.Cancel();
            _gameTimerSource?.Dispose();
            _sacrificeSource?.Cancel();
            _sacrificeSource?.Dispose();
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Unsubscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
        }

        private void OnGameStarted()
        {
            if (_sacrificeSettings.EnableSacrifices)
            {
                StartCoroutine(SacrificeCoroutine());
            }
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
        private void OnCityDestroyedWhileSarifice()
        {
            _sacrificeSource?.Cancel();
            _sacrificeSource?.Dispose();

#if UNITY_EDITOR
            _logger.Log("<b><color=yellow>Sacrifice Offer</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
#endif
        }
        
        private async void SetApocalypseTimer()
        {
            try
            {
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
            float probability = MathUtils.Random.NextFloat();
            if (probability > _sacrificeSettings.Probability) return;

            // Get random sacrifice
            int rand = MathUtils.Random.NextInt(0, _sacrifices.Value.Length - 1);
            SacrificeModel sacrifice = _sacrifices.Value[rand];
            
            var form = (SacrificeForm)SacrificeForm.CreateForm(sacrifice, city);

#if UNITY_EDITOR
            _logger.Log($"<b><color=yellow>Sacrifice Offer</color></b> in <b><color=yellow>{city.name}</color></b>.", LogType.Game);
            form.Elapsed += () => _logger.Log($"Player <b><color=yellow>ignored</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif

            // If city was destroyed, abort the sacrifice offer
            city.Destroyed += OnCityDestroyedWhileSarifice;

            // Wait for player's decision
            form.StartTimer(_sacrificeSettings.Duration);
            bool accepted = await form.AwaitForConfirm(_sacrificeSource.Token);
            city.Destroyed -= OnCityDestroyedWhileSarifice;

#if UNITY_EDITOR
            _logger.Log($"Player <b>{(accepted ? "<color=green>accepted" : "<color=red>denied")}</color></b> the sacrifice from <b><color=yellow>{city.name}</color></b>.", LogType.Game);
#endif
        }
        private void StartApocalypse()
        {
            _signalBus.Fire<GameApocalypseSignal>();
        }
        private IEnumerator SacrificeCoroutine()
        {
            while (true)
            {
                float time = MathUtils.Random.NextFloat(_sacrificeSettings.AppearenceInterval.x, _sacrificeSettings.AppearenceInterval.y);
                yield return new WaitForSecondsRealtime(time);
                CityScript city = _mapController.Cities
                    .SelectMany(city => city.PriestsAmount >= _sacrificeSettings.SacrificeThreshold)
                    .Randomly();
                
                if (city != null) OfferSacrificeInCity(city);
            }
        }
    }
}
