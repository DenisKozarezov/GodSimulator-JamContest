using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _abilityName;
        [SerializeField]
        private TextMeshProUGUI _abilityCooldown;
        [SerializeField]
        private TextMeshProUGUI _abilityDescription;

        public string Name { get => _abilityName.text; set => _abilityName.text = value; }
        public float Cooldown { set => _abilityCooldown.text = $"Cooldown: {value}"; }
        public string Description { get => _abilityDescription.text; set => _abilityDescription.text = value; }
    }
}