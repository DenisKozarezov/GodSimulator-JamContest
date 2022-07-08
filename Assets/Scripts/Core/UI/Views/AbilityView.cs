using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using Core.Models;

namespace Core.UI
{
    public class AbilityView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Image _cooldownImage;
        [SerializeField]
        private TextMeshProUGUI _cooldownText;

        private bool _ready = true;
        private float _timer;
        private AbilityModel _model;

        public event Action Execute;
        public event Action MouseEnter;
        public event Action MouseExit;

        public void SetAbility(AbilityModel model)
        {
            _model = model;
            _button.image.sprite = model.Icon;
            _cooldownImage.sprite = model.Icon;
        }

        private void Start()
        {
            _cooldownText.gameObject.SetActive(false);
            _button.onClick.AddListener(OnClick);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        private void OnClick()
        {
            if (!_ready) return;

            Execute?.Invoke();

            StartCooldown(_model.Cooldown);
        }    
        private void SetTimer(float time)
        {
            _timer = time;
            _cooldownText.text = Math.Round(time, 1).ToString();
        }
        private void StartCooldown(float cooldown)
        {
            if (cooldown < 0f) throw new ArgumentOutOfRangeException();

            _ready = false;
            _cooldownText.gameObject.SetActive(true);

            _cooldownImage.fillAmount = 0f;
            _cooldownImage.DOFillAmount(1f, cooldown);
            DOTween.To(() => _timer = cooldown, x=> SetTimer(x), 0f, cooldown)
            .OnComplete(() =>
            {
                _ready = true;
                _cooldownText.gameObject.SetActive(false);
            });
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            MouseEnter?.Invoke();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            MouseExit?.Invoke();
        }
    }
}