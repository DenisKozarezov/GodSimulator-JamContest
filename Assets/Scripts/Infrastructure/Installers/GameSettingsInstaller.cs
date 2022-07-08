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

        public float GameTime => _gameTime;
        public PlayerSettings PlayerSettings => _playerSettings;

        public override void InstallBindings()
        {
            Container.Bind<ILogger>().FromInstance(new StandaloneLogger()).AsSingle();
            Container.BindInstances(_playerSettings);
        }
    }
}