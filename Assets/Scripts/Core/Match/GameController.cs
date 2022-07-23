using System.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using Core.Infrastructure;

namespace Core.Match
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _chooseForm;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            SceneController.SceneLoaded += OnSceneLoaded;
            _signalBus.Subscribe<SceneLoadedSignal>(OpenChooseForm);
        }
        private void OnDestroy()
        {
            SceneController.SceneLoaded -= OnSceneLoaded;
            _signalBus.Unsubscribe<SceneLoadedSignal>(OpenChooseForm);
        }
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            _signalBus.Fire(new SceneLoadedSignal { Scene = scene });
        }

        public async void OpenChooseForm()
        {
            var form = Instantiate(_chooseForm).GetComponentInChildren<UI.Forms.ChooseForm>();
            await form.AwaitForConfirm();

            await Task.Delay(TimeSpan.FromSeconds(1f));
            form.Close();

            _signalBus.Fire<GameStartedSignal>();
        }
    }
}