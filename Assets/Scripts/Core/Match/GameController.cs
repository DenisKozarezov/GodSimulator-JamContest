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
        private SignalBus _signalBus;
        private LinkedList<Player> _players = new LinkedList<Player>();
        public IReadOnlyCollection<Player> Players => _players;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            SceneController.SceneLoaded += OnSceneLoaded;        
        }
        private void OnDestroy()
        {
            SceneController.SceneLoaded -= OnSceneLoaded;        
        }
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            _signalBus.Fire(new SceneLoadedSignal { Scene = scene });
        }   
    }
}