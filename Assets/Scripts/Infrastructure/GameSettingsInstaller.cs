using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public override void InstallBindings()
        {
            
        }
    }
}