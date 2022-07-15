using Zenject;

namespace Core.Infrastructure
{
    public class UISignalsInstaller : Installer<UISignalsInstaller>
    {
        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<SelectionModeChangedSignal>();
            Container.DeclareSignal<TempleDragBeginSignal>();
            Container.DeclareSignal<TempleDragEndSignal>();
        }
    }
}
