using Zenject;

namespace Core.Infrastructure
{
    public struct GameStartedSignal { }
    public struct PlayerVictorySignal { }
    public struct GodParametersChangedSignal
    {
        public float War;
        public float Nature;
        public float Love;

        public override string ToString()
        {
            return $"<b><color=red>War</color></b>: {War}, " +
                   $"<b><color=yellow>Nature</color></b>: {Nature}, " +
                   $"<b><color=green>Love</color></b>: {Love}.";
        }
    }

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


            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("Fired GameStartedSignal", LogType.Game));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log("Fired PlayerVictorySignal", LogType.Game));
            Container.BindSignal<GodParametersChangedSignal>().ToMethod((x) => Logger.Log(x.ToString(), LogType.Game));
        }
    }
}