using UnityEngine;
using TMPro;
using Core.UI.Forms;

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

        [Header("Form")]
        [SerializeField]
        private bool _autoSize;
        [SerializeField, Min(0f)]
        private float _minHeight;

        public string Name { get => _abilityName.text; set => _abilityName.text = value; }
        public float Cooldown { set => _abilityCooldown.text = $"Cooldown: {value}"; }
        public string Description { get => _abilityDescription.text; set => _abilityDescription.text = value; }

        public float MinHeight => _minHeight;
        public bool AutoSize => _autoSize;
    }
}