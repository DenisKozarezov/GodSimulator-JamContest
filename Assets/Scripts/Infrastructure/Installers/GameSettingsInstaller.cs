using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Loading;
using Core.Input;
using Editor;

namespace Core.Models
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [Header("Settings")]
        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private AudioSettings _audioSettings;
        [SerializeField]
        private UISettings _UISettings;
        [Space, SerializeField]
        private GameSettings _gameSettings;
        [SerializeField]
        private SacrificeSettings _sacrificeSettings;
        
        [Serializable]
        public class GameSettings
        {
            [SerializeField, Min(0)]
            private int _gameTime;
            [SerializeField, Min(0)]
            private int _virtueLevels;
            [SerializeField]
            private bool _enableFogOfWar;
            [SerializeField, TextArea(minLines: 10, maxLines: 20)]
            private string _citiesNames;
            private Queue<string> _names;

            public int GameTime => _gameTime;
            public int VirtueLevels => _virtueLevels;
            public bool EnableFogOfWar => _enableFogOfWar;
            public Queue<string> CitiesNames
            {
                get
                {
                    if (_names == null) _names = new Queue<string>(_citiesNames.Split('\n'));
                    return _names;
                }
            }
        }
        [Serializable]
        public class SacrificeSettings
        {
            [SerializeField, MinMaxSlider(0f, 200f)]
            private Vector2Int _appearenceInterval;
            [SerializeField, Min(0f)]
            private float _duration;
            [SerializeField, Range(0f, 1f)]
            private float _probability;
            [SerializeField]
            private SacrificeModel[] _enabledSacrifices;
            public Vector2Int AppearenceInterval => _appearenceInterval;
            public float Duration => _duration;
            public float Probability => _probability;
            public IReadOnlyCollection<SacrificeModel> EnabledSacrifices => _enabledSacrifices;
        }

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<ILogger>().To<StandaloneLogger>().AsCached();
            Container.Bind<ILoadingProvider>().To<LoadingProvider>().AsCached();
            Container.Bind<IInputSystem>().To<StandaloneInput>().AsCached();

            Container.BindInstance(_playerSettings).IfNotBound();
            Container.BindInstance(_audioSettings).IfNotBound();
            Container.BindInstance(_UISettings).IfNotBound();
            Container.BindInstance(_gameSettings).IfNotBound();
            Container.BindInstance(_sacrificeSettings).IfNotBound();
        }     
    }
}