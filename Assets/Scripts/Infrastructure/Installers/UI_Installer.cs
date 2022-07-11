using Core.UI;
using Core.UI.Forms;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class UI_Installer : MonoInstaller
    {    
        public override void InstallBindings()
        {
            UISignalsInstaller.Install(Container);

            Container.Bind<UIController>().AsSingle();
        }
    }
}
