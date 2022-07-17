using Core.UI;
using Core.UI.Forms;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class UI_Installer : MonoInstaller
    {
        [SerializeField]
        private GameObject _movingPriestsPanel;

        public override void InstallBindings()
        {
            UISignalsInstaller.Install(Container);

            Container.Bind<UIContoller>().AsSingle();

            Container.Bind<MovingPriestsForm>().FromComponentInNewPrefab(_movingPriestsPanel).AsSingle();

            Container.BindSignal<PlayerClickedOnCitySignal>()
                .ToMethod<MovingPriestsForm>(x => x.InitializePanel).FromResolve();

            Container.BindSignal<SelectionModeChangedSignal>()
                .ToMethod<UIContoller>(x => x.SetSelectionMode).FromResolve();
        }
    }
}
