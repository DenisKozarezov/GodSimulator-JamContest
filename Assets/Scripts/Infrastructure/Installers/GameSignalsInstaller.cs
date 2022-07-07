using Zenject;

namespace Core.Infrastructure
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        [Inject]
        private ILogger Logger;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Declare all signals
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<PlayerVictorySignal>();
            Container.DeclareSignal<GodParametersChangedSignal>();
            Container.DeclareSignalWithInterfaces<PlayerClickedOnCitySignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("Fired GameStartedSignal.", LogType.Game));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log("Fired PlayerVictorySignal.", LogType.Game));
            Container.BindSignal<GodParametersChangedSignal>().ToMethod((x) => Logger.Log(x.ToString(), LogType.Game));
            Container.BindSignal<PlayerClickedOnCitySignal>().ToMethod((x) => Logger.Log($"Player clicked on {x.View}.", LogType.Game));
#endif
        }
    }
}