using Zenject;

namespace Core.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {            
            // Bind signals
            GameSignalsInstaller.Install(Container);

            Container.Bind<MovingBetweenCities>().AsSingle();

            Container.BindSignal<PlayerWantToMovingPriestsSignal>()
                .ToMethod<MovingBetweenCities>(x => x.ShowRange).FromResolve();
        }
    }
}