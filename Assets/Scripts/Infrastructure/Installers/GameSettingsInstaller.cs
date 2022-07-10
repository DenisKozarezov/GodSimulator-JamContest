using System;
using UnityEngine;
using Zenject;
using Core.Input;
using System.Collections.Generic;

namespace Core.Models
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [Serializable]
        public class GameSettings
        {
            [SerializeField, Min(0)]
            private int _gameTime;
            [SerializeField, Min(0)]
            private int _virtueLevels;
            [SerializeField, TextArea(minLines: 10, maxLines: 20)]
            private string _citiesNames;
            private Stack<string> _names;

            public int GameTime => _gameTime;
            public int VirtueLevels => _virtueLevels;
            public Stack<string> CitiesNames
            {
                get
                {
                    if (_names == null) _names = new Stack<string>(_citiesNames.Split('\n'));
                    return _names;
                }
            }
        }

        [Header("Settings")]
        [SerializeField]
        private GameSettings _gameSettings;
        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private AudioSettings _audioSettings;

        public override void InstallBindings()
        {
            Container.Bind<ILogger>().FromInstance(new StandaloneLogger()).AsSingle();
            Container.Bind<IInputSystem>().FromInstance(new StandaloneInput()).AsSingle();
            Container.BindInstances(_gameSettings, _playerSettings, _audioSettings);
        }
    }
}