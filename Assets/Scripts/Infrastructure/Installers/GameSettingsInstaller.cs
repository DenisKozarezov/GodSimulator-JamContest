using UnityEngine;
using Zenject;

namespace Core.Models
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [Header("Settings")]
        [SerializeField, Min(0)]
        private int _gameTime;
        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private AudioSettings _audioSettings;

        public float GameTime => _gameTime;

        public override void InstallBindings()
        {
            Container.Bind<ILogger>().FromInstance(new StandaloneLogger()).AsSingle();
            Container.BindInstances(_playerSettings, _audioSettings);
        }
    }
}