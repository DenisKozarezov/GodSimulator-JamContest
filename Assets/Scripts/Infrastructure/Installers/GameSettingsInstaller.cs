using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Loading;
using Core.Input;
using Editor;

namespace Core.Models
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField, Min(0)]
        private int _gameTime;
        [SerializeField, Min(0)]
        private int _virtueLevels;
        [SerializeField]
        private bool _enableFogOfWar;
        [SerializeField]
        private bool _randomStartCitySelect;
        [SerializeField, TextArea(minLines: 10, maxLines: 20)]
        private string _citiesNames;
        private Queue<string> _names;
        public int GameTime => _gameTime;
        public int VirtueLevels => _virtueLevels;
        public bool EnableFogOfWar => _enableFogOfWar;
        public bool RandomStartCitySelect => _randomStartCitySelect;
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
        [SerializeField, Tooltip("Enable or disable a sacrifice offering in a match.")]
        private bool _enableSacrifices;
        [SerializeField, MinMaxSlider(0f, 90f), Tooltip("The interval of a sacrifice offering appearence in cities.")]
        private Vector2Int _appearenceInterval;
        [SerializeField, Tooltip("The minimum number of priests required for a sacrifice offering.")]
        private ushort _sacrificeThreshold;
        [SerializeField, Min(0f), Tooltip("The duration of a sacrifice offering. Players can accept or deny it (otherwise it will be ignored).")]
        private float _duration;
        [SerializeField, Range(0f, 1f), Tooltip("The probability of a sacrifice offering in cities.")]
        private float _probability;
        public bool EnableSacrifices => _enableSacrifices;
        public Vector2Int AppearenceInterval => _appearenceInterval;
        public ushort SacrificeThreshold => _sacrificeThreshold;
        public float Duration => _duration;
        public float Probability => _probability;
    }


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