using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Infrastructure;
using Core.Models;

namespace Core.UI
{
    public class VirtuesPanel : MonoBehaviour
    {
        [SerializeField]
        private VirtueView[] _virtueViews;
        private Dictionary<uint, VirtueView> _virtues = new Dictionary<uint, VirtueView>();
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus, PlayerSettings playerSettings)
        {
            int index = 0;
            foreach (var virtue in playerSettings.Virtues)
            {
                _virtueViews[index].SetIcon(virtue.Icon);
                _virtues.Add(virtue.ID, _virtueViews[index]);
                index++;
            }
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<PlayerVirtueChangedSignal>(OnVirtueChanged);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerVirtueChangedSignal>(OnVirtueChanged);
        }

        private void OnVirtueChanged(PlayerVirtueChangedSignal param)
        {
            _virtues[param.Virtue.ID].SetFillAmount(param.State.Percent / Constants.VirtueValueMax);
        }
    }
}