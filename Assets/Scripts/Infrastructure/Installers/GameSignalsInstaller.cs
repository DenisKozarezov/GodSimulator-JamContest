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
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("GameStartedSignal", LogType.Signal));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log("PlayerVictorySignal", LogType.Signal));
            Container.BindSignal<GodParametersChangedSignal>().ToMethod((x) => Logger.Log(x.ToString(), LogType.Game));
            Container.BindSignal<PlayerClickedOnCitySignal>().ToMethod((x) => Logger.Log($"Player clicked on <b><color=yellow>{x.View}</color></b>.", LogType.Game));
#endif
        }
    }
}