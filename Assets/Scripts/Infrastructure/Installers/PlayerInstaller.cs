using Zenject;

namespace Core.Infrastructure
{
    public class PlayerInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

        public override void InstallBindings()
        {
            Container.DeclareSignal<PlayerVictorySignal>();
            Container.DeclareSignal<PlayerVirtueChangedSignal>();
            Container.DeclareSignal<PlayerMovingPriestsSignal>();
            Container.DeclareSignal<PlayerClickedOnAbilitySignal>();
            Container.DeclareSignalWithInterfaces<PlayerCastedTargetAbilitySignal>();
            Container.DeclareSignalWithInterfaces<PlayerCastedNonTargetAbilitySignal>();
            Container.DeclareSignalWithInterfaces<PlayerClickedOnCitySignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<PlayerVictorySignal>().ToMethod(() => Logger.Log(typeof(PlayerVictorySignal).Name, LogType.Signal));
            Container.BindSignal<PlayerMovingPriestsSignal>().ToMethod((x) => Logger.Log($"Player is moving <b><color=yellow>{x.PriestsAmount}</color></b> priests from <b><color=yellow>{x.Temple.name}</color></b> to <b><color=yellow>{x.Target.name}</color></b>.", LogType.Game));
            Container.BindSignal<PlayerClickedOnCitySignal>().ToMethod((x) => Logger.Log($"Player clicked on <b><color=yellow>{x.View}</color></b>.", LogType.Game));
            Container.BindSignal<IPlayerCastedAbility>().ToMethod((x) => Logger.Log($"Player used <b><color=yellow>{x.Ability.DisplayName}</color></b> ability.", LogType.Game));
#endif
        }
    }
}