using UnityEngine;
using Zenject;
using Core.Cities;
using Core.Input;
using Core.Match;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

        [SerializeField]
        private MapController _mapController;
        [SerializeField]
        private GameEventsController _eventsController;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<SceneLoadedSignal>();
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<GameApocalypseSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<SceneLoadedSignal>().ToMethod(() => Logger.Log("SceneLoadedSignal", LogType.Signal));
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("GameStartedSignal", LogType.Signal));
            Container.BindSignal<GameApocalypseSignal>().ToMethod(() => Logger.Log("GameApocalypseSignal", LogType.Signal));
#endif

            Container.Bind<MapController>().FromInstance(_mapController).AsSingle();
            Container.Bind<GameEventsController>().FromInstance(_eventsController).AsSingle();
            Container.BindInterfacesAndSelfTo<MovingBetweenCities>().AsSingle();
            Container.Bind<ITickable>().To<StandaloneInput>().AsCached();
        }
    }
}