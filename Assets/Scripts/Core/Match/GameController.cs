using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using Core.Models;

namespace Core.Match
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _chooseForm;

        private SignalBus _signalBus;
        private GameEventsController _eventsController;
        private bool _randomStartCitySelect;
        private LinkedList<Player> _players = new LinkedList<Player>();
        public IReadOnlyCollection<Player> Players => _players;

        [Inject]
        private void Construct(SignalBus signalBus, GameEventsController eventsController, GameSettings settings)
        {
            _signalBus = signalBus;
            _eventsController = eventsController;
            _randomStartCitySelect = settings.RandomStartCitySelect;
        }

        private void Awake()
        {
            SceneController.SceneLoaded += OnSceneLoaded;
            _signalBus.Subscribe<SceneLoadedSignal>(SelectStartVirtueAndCity);         
        }
        private void OnDestroy()
        {
            SceneController.SceneLoaded -= OnSceneLoaded;
            _signalBus.Unsubscribe<SceneLoadedSignal>(SelectStartVirtueAndCity);       
        }
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            _signalBus.Fire(new SceneLoadedSignal { Scene = scene });
        }
        private async void SelectStartVirtueAndCity()
        {
            var form = Instantiate(_chooseForm).GetComponentInChildren<UI.Forms.ChooseForm>();
            await form.AwaitForConfirm();

            if (!_randomStartCitySelect)
            {
                var selectedCity = await _eventsController.WaitForStartCitySelection();
                selectedCity.BuildTemple(null);
                selectedCity.SetOwner(new Player());
            }

            await Task.Delay(TimeSpan.FromSeconds(1f));

            _signalBus.Fire<GameStartedSignal>();
        }
    }
}