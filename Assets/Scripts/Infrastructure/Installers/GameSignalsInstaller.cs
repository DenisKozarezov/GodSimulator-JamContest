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
            Container.DeclareSignal<PlayerWantToMovingPriestsSignal>();
            Container.DeclareSignal<PlayerVirtueChangedSignal>();
            Container.DeclareSignal<UISignals.MovingModeChangedSignal>();
            Container.DeclareSignalWithInterfaces<PlayerUsedTargetAbilitySignal>();
            Container.DeclareSignalWithInterfaces<PlayerUsedNonTargetAbilitySignal>();
            Container.DeclareSignalWithInterfaces<PlayerUsedAreaAbilitySignal>();

            Container.DeclareSignalWithInterfaces<PlayerClickedOnCitySignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameStartedSignal>().ToMethod(() => Logger.Log("GameStartedSignal", LogType.Signal));
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log("PlayerVictorySignal", LogType.Signal));
            Container.BindSignal<PlayerClickedOnCitySignal>().ToMethod((x) => Logger.Log($"Player clicked on <b><color=yellow>{x.View}</color></b>.", LogType.Game));
            Container.BindSignal<IPlayerUsedAbility>().ToMethod((x) => Logger.Log($"Player used <b><color=yellow>{x.Ability.DisplayName}</color></b> ability.", LogType.Game));
            Container.BindSignal<PlayerWantToMovingPriestsSignal>().ToMethod(() => Logger.Log("PlayerWantToMovingPriestsSignal", LogType.Signal));
#endif
        }
    }
}