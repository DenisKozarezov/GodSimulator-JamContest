using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public struct GameStartedSignal { }
    public struct PlayerVictorySignal { }

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

            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("Fired GameStartedSignal", LogType.Game));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log("Fired PlayerVictorySignal", LogType.Game));
        }
    }
}