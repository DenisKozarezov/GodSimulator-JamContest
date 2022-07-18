using Editor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
            [SerializeField, MinMaxSlider(0f, 200f)]
            private Vector2Int _sacrificeAppearenceTime;
            [SerializeField, Min(0f)]
            private float _sacrificeDuration;
            [SerializeField, TextArea(minLines: 10, maxLines: 20)]
            private string _citiesNames;
            private Queue<string> _names;

            public int GameTime => _gameTime;
            public int VirtueLevels => _virtueLevels;
            public Vector2Int SacrificeAppearenceTime => _sacrificeAppearenceTime;
            public float SacrificeDuration => _sacrificeDuration;
            public Queue<string> CitiesNames
            {
                get
                {
                    if (_names == null) _names = new Queue<string>(_citiesNames.Split('\n'));
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
        [SerializeField]
        private UISettings _UISettings;

        public override void InstallBindings()
        {
            Container.Bind<ILogger>().FromInstance(new StandaloneLogger()).AsSingle();
            Container.BindInstances(_gameSettings, _playerSettings, _audioSettings, _UISettings);
        }
    }
}