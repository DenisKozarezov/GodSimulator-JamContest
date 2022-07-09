using UnityEngine;
using Zenject;
using Core.Infrastructure;

namespace Core.UI
{
    public class VirtuesPanel : MonoBehaviour
    {
        [SerializeField]
        private VirtueView _war;
        [SerializeField]
        private VirtueView _nature;
        [SerializeField]
        private VirtueView _love;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<PlayerVirtueChangedSignal>(OnParametersChanged);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerVirtueChangedSignal>(OnParametersChanged);
        }

        private void OnParametersChanged(PlayerVirtueChangedSignal param)
        {
            _war.SetFillAmount(param.War);
            _nature.SetFillAmount(param.Nature);
            _love.SetFillAmount(param.Love);
        }
    }
}