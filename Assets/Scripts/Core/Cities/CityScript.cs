using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Zenject;
using RotaryHeart.Lib.SerializableDictionary;
using Core.Infrastructure;
using Core.Models;
using DG.Tweening;
using static Core.Models.GameSettingsInstaller;

namespace Core.Cities
{
    public class CityScript : InteractableView
    {
        public enum State : byte
        {
            CityFree = 0x00,
            CityWithTemple = 0x01,
            CityDestroyed = 0x02
        }

        [SerializeField]
        private TextMeshPro _name;
        [SerializeField]
        private PranaView _pranaView;

        private float _timer;
        private bool _interactable = true;
        private ICityStrategy CurrentStrategy;
        private SerializableDictionaryBase<GodModel, byte> _percentageOfFaithful;
        private GodModel _invader;

        public override bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (CurrentStrategy != null) CurrentStrategy.Interactable = value;
            }
        }

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            if (gameSettings.CitiesNames.Count > 0)
            {
                string name = gameSettings.CitiesNames.Dequeue();
                _name.text = name;
                gameObject.name = name + " City";
            }
        }       

        protected override void Start()
        {
            CurrentStrategy = GetComponent<ICityStrategy>();
            _percentageOfFaithful = new SerializableDictionaryBase<GodModel, byte>();
            Interactable = true;

            if (_pranaView == null) return;
            DOTween.To(() => 0f, (x) => _pranaView.SetFillAmount(x), 1f, 15f).SetEase(Ease.Linear);
        }

        public void BuildTemple(VirtueModel virtue)
        {
            TempleStrategy temple = gameObject.AddComponent<TempleStrategy>();
            temple.SetVirtue(virtue);
            CurrentStrategy = temple;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;

            SignalBus.Fire(new PlayerClickedOnCitySignal { View = this });
        }
    }
}