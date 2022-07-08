using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Canvas _canvas;

        public override void InstallBindings()
        {            
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();

            // Bind signals
            GameSignalsInstaller.Install(Container);
        }
    }
}