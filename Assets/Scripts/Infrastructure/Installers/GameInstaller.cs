using Zenject;
using Core.Cities;
using Core.Input;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

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

            Container.BindInterfacesTo<MovingBetweenCities>().AsSingle();

            Container.BindInterfacesAndSelfTo<StandaloneInput>().AsSingle();
        }
    }
}