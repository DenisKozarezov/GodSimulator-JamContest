using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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

        private IEnumerable<CityScript> _cities;
        private Lazy<SacrificeModel[]> _sacrifices;

        [SerializeField]
        private GameObject _chooseForm;

        private TaskCompletionSource<CityScript> _selectStartCitySource;
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

#if UNITY_EDITOR
            Assert.IsNotNull(_chooseForm);
#endif
        }
        private void Start()
        {
            _signalBus.Subscribe<SceneLoadedSignal>(SelectStartVirtueAndCity);
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Subscribe<PlayerBuildingTempleSignal>(OnPlayerBuildingTemple);      
            _signalBus.Subscribe<PlayerClickedOnCitySignal>(OnPlayerSelectedStartCity);
        }
        private void OnDestroy()
        {
            _gameTimerSource?.Cancel();
            _gameTimerSource?.Dispose();
            _sacrificeSource?.Cancel();
            _sacrificeSource?.Dispose();
            _signalBus.Unsubscribe<SceneLoadedSignal>(SelectStartVirtueAndCity);
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<GameStartedSignal>(SetApocalypseTimer);
            _signalBus.Unsubscribe<PlayerBuildingTempleSignal>(OnPlayerBuildingTemple);
            _signalBus.Unsubscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
            _signalBus.TryUnsubscribe<PlayerClickedOnCitySignal>(OnPlayerSelectedStartCity);
        }

        private void OnGameStarted()
        {
            _signalBus.Subscribe<PlayerCastedTargetAbilitySignal>(OnPlayerCastedAbility);
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
        private void OnPlayerSelectedStartCity(PlayerClickedOnCitySignal signal)
        {
            if (_selectStartCitySource != null && _cities.Contains(signal.View))
            {
#if UNITY_EDITOR
                _logger.Log($"Player selected started city <b><color=yellow>{signal.View.name}</color></b>.", LogType.Game);
#endif
                _selectStartCitySource.SetResult(signal.View);
                _selectStartCitySource = null;
                _signalBus?.Unsubscribe<PlayerClickedOnCitySignal>(OnPlayerSelectedStartCity);
            }
        }
        private void OnPlayerBuildingTemple(PlayerBuildingTempleSignal signal)
        {
            signal.City.SetOwner(signal.Player);
            signal.City.BuildTemple(null);
            signal.City.ClearPriests();
        }
        private void OnCityDestroyedWhileSarifice()
        {
            _sacrificeSource?.Cancel();
            _sacrificeSource?.Dispose();

#if UNITY_EDITOR
            _logger.Log("<b><color=yellow>Sacrifice Offer</color></b> task was <b><color=yellow>cancelled</color></b>.", LogType.Warning);
#endif
        }
        
        private void StartApocalypse()
        {
            _signalBus.Fire<GameApocalypseSignal>();
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
        private async void SelectStartVirtueAndCity()
        {
            var form = Instantiate(_chooseForm).GetComponentInChildren<UI.Forms.ChooseForm>();
            await form.AwaitForConfirm();

            if (!_gameSettings.RandomStartCitySelect)
            {
                var selectedCity = await WaitForStartCitySelection();
                _signalBus.Fire(new PlayerBuildingTempleSignal
                {
                    City = selectedCity,
                    Player = GameController.MainPlayer
                });
            }

            await Task.Delay(TimeSpan.FromSeconds(1f));

            _signalBus.Fire<GameStartedSignal>();
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
        public async Task<CityScript> WaitForStartCitySelection()
        {
            // Select cities on the outer rim
            _cities = _mapController.Cities.Jarvis();
            foreach (CityScript city in _cities)
            {
                city.Interactable = true;
                city.Select(InteractableView.SelectType.Strong);
            }

            // Wait for selection
            _signalBus.Fire<PlayerSelectingStartCitySignal>();
            _selectStartCitySource = new TaskCompletionSource<CityScript>();
            CityScript result = await Task.Run(() => _selectStartCitySource.Task, _gameTimerSource.Token);
            _signalBus.Fire(new PlayerSelectedStartCitySignal { View = result });

            // Make all others cities interactable
            foreach (CityScript city in _cities)
            {
                city.Deselect();
            }
            _mapController.SetAllCitiesInteractable(true);
            return result;
        }
    }
}
