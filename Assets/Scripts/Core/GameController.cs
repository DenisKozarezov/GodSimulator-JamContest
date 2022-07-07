using UnityEngine;
using Zenject;
using Core.Infrastructure;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            _signalBus.Fire<GameStartedSignal>();
        }
    }
}