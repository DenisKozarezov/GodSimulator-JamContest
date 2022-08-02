using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Infrastructure;

namespace Core.Match
{
    public class GameController : MonoBehaviour
    {
        private SignalBus _signalBus;

        private Player.Factory _playerFactory;
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
        public static Player MainPlayer { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus, Player.Factory playerFactory)
        {
            _signalBus = signalBus;
            _playerFactory = playerFactory;
        }

        private void Awake()
        {
            SceneController.SceneLoaded += OnSceneLoaded;        
        }
        private void Start()
        {
            _signalBus.Subscribe<IPlayerCastedAbility>(OnAbilityCasted);
            AddPlayer(Color.green);
            MainPlayer = _players.Values.First();
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<IPlayerCastedAbility>(OnAbilityCasted);
            _players.Clear();
            SceneController.SceneLoaded -= OnSceneLoaded;        
        }
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            _signalBus.Fire(new SceneLoadedSignal { Scene = scene });
        }
        private void OnAbilityCasted(IPlayerCastedAbility ability)
        {
            string GUID = ability.Player.GUID;
            if (_players.TryGetValue(GUID, out Player player))
            {
                var influencer = ability.Ability.VirtuesInfluencer;
                foreach (var virtue in influencer.BuffedVirtues)
                {
                    player.AddVirtueValue(virtue.Virtue, virtue.Value);
                }
                foreach (var virtue in influencer.DebuffedVirtues)
                {
                    player.ReduceVirtueValue(virtue.Virtue, virtue.Value);
                }
            }
        }

        private Player AddPlayer(Color color)
        {
            Player player = _playerFactory.Create(Guid.NewGuid().ToString());
            player.Color = color;
            _players.Add(player.GUID, player);
            return player;
        }
    }
}