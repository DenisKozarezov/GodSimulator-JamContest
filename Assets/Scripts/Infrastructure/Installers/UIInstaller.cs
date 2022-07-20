using UnityEngine;
using Core.UI;
using Core.UI.Forms;
using Zenject;

namespace Core.Infrastructure
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            UISignalsInstaller.Install(Container);
        }
    }
}
