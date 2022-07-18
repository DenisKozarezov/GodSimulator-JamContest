using Core.UI;
using Core.UI.Forms;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _movingPriestsForm;
        [SerializeField]
        private GameObject _templeRadius;
        [SerializeField]
        private GameObject _movingPriestsIcon;
        [SerializeField]
        private GameObject _animatedDottedLine;

        public override void InstallBindings()
        {
            UISignalsInstaller.Install(Container);

            Container.Bind<MovingPriestsForm>().FromInstance(_movingPriestsForm.GetComponent<MovingPriestsForm>()).AsCached();
            Container.Bind<TempleRadius>().FromInstance(_templeRadius.GetComponent<TempleRadius>()).AsCached();
            Container.Bind<MovingPriestsIcon>().FromInstance(_movingPriestsIcon.GetComponent<MovingPriestsIcon>()).AsCached();
            Container.Bind<AnimatedDottedLine>().FromInstance(_animatedDottedLine.GetComponent<AnimatedDottedLine>()).AsCached();
        }
    }
}
