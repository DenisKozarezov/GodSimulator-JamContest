using UnityEngine;
using Zenject;
using Core.Cities;
using Core.Match;
using Core.Input;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;
        [Inject]
        private IInputSystem InputSystem;

        [SerializeField]
        private MapController _mapController;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Declare all signals
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<GameApocalypsisSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("GameStartedSignal", LogType.Signal));
            Container.BindSignal<GameApocalypsisSignal>().ToMethod(() => Logger.Log("GameApocalypsisSignal", LogType.Signal));
#endif

            Container.Bind<MapController>().FromInstance(_mapController).AsSingle();
            Container.BindInterfacesTo<GameEventsController>().AsSingle();
            Container.BindInterfacesTo<MovingBetweenCities>().AsSingle();
            Container.BindInterfacesTo<IInputSystem>().AsSingle();
        }
    }
}