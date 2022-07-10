using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Core.Models.GameSettingsInstaller;

namespace Core.UI
{
    public class VirtueView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _bar;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private RectTransform _separator;
        private RectTransform _rectTransform;

        private int DivisionsCount;

        [Inject]
        public void Construct(GameSettings _gameSettings)
        {
            DivisionsCount = _gameSettings.VirtueLevels + 1;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SeparateBarOnLevels(DivisionsCount);

            // Create final division with ultimate wonder
            float lastDivision = 1f - (1f / DivisionsCount);
            float factor = Mathf.Lerp(lastDivision, 1f, 0.5f);
            var wonderSeparator = CreateSeparator(factor);
            wonderSeparator.sizeDelta += Vector2.up * 10f;
            wonderSeparator.GetComponent<RawImage>().color = Color.white;
        }
        private RectTransform CreateSeparator(float factor, int index = 0, bool hasText = true)
        {
            var separator = Instantiate(_separator, _rectTransform);
            separator.name = "Separator";

            float posX = _rectTransform.rect.width * factor;
            separator.anchoredPosition = new Vector2(posX, separator.anchoredPosition.y);
            string roman = hasText ? Utils.ParseToRoman(index) : string.Empty;
            separator.GetComponentInChildren<TextMeshProUGUI>().text = roman;
            return separator;
        }
        private void SeparateBarOnLevels(int levelsCount)
        {
            float factor = 1f / (float)levelsCount;
            for (int index = 0; index <= levelsCount; index++)
            {
                CreateSeparator(index * factor, index, index != levelsCount);
            }
        }
        
        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
        public void SetFillAmount(float fillAmount)
        {
            float width = Mathf.Lerp(_rectTransform.rect.width, 0f, fillAmount);
            _bar.offsetMax = Vector2.left * width;
        }
    }
}