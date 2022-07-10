using Core.Cities;
using Core.Input;
using Zenject;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {            
            // Bind signals
            GameSignalsInstaller.Install(Container);

            Container.BindInterfacesTo<Player>().AsSingle();
            Container.Bind<MovingBetweenCities>().AsSingle();

            Container.BindInterfacesAndSelfTo<StandaloneInput>().AsSingle();

            Container.BindSignal<PlayerWantToMovingPriestsSignal>()
                .ToMethod<MovingBetweenCities>(x => x.ShowRange).FromResolve();
        }
    }
}