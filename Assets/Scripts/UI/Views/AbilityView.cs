using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Core.UI
{
    public class AbilityView : MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Image _cooldownImage;
        [SerializeField]
        private TextMeshProUGUI _cooldownText;

        private float _timer;

        private void SetTimer(float time)
        {
            _timer = time;
            _cooldownText.text = Math.Round(time, 1).ToString();
        }

        public void SetIcon(Sprite icon)
        {
            _backgroundImage.sprite = icon;
        }
        public void StartCooldown(float cooldown)
        {
            if (cooldown < 0f) throw new ArgumentOutOfRangeException();

            _cooldownImage.fillAmount = 0f;
            _cooldownImage.DOFillAmount(1f, cooldown);
            DOTween.To(() => _timer = cooldown, x=> SetTimer(x), 0f, cooldown);
        }
    }
}