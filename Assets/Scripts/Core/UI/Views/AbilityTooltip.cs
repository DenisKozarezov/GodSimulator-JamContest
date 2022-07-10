using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.UI.Forms;
using Core.Models;

namespace Core.UI
{
    public class AbilityTooltip : MonoBehaviour, IAutoSizable
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI _abilityName;
        [SerializeField]
        private TextMeshProUGUI _abilityCooldown;
        [SerializeField]
        private TextMeshProUGUI _abilityDescription;
        [SerializeField]
        private TextMeshProUGUI _abilityType;
        [SerializeField]
        private Image _virtue;

        [Header("Form")]
        [SerializeField]
        private bool _autoSize;
        [SerializeField, Min(0f)]
        private float _minHeight;

        private RectTransform _rectTransform;

        public string Name { set => _abilityName.text = value; }
        public float Cooldown { set => _abilityCooldown.text = $"Cooldown: {value}"; }
        public string Description { set => _abilityDescription.text = value; }
        public AbilityType AbilityType { set => _abilityType.text = value.ToString(); }
        public Image VirtueIcon { get => _virtue; set => _virtue = value; }

        public float MinHeight => _minHeight;
        public bool AutoSize => _autoSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        public void SetPosition(Vector2 position)
        {
            _rectTransform.position = position;
        }
    }
}