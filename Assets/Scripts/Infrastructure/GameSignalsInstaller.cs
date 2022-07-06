using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public struct GameStartedSignal { }
    public struct PlayerVictorySignal { }

    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Declare all signals
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<PlayerVictorySignal>();

            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Debug.Log("Fired GameStartedSignal"));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Debug.Log("Fired PlayerVictorySignal"));
        }
    }
}