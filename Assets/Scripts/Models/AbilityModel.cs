using System;
using UnityEngine;
using Editor;

namespace Core.Models
{
    public enum AbilityType : byte
    {
        NonTarget = 0x00,
        Target = 0x01,
    }

    [Flags]
    public enum AbilityUsage : byte
    {
        Friendly = 0x00,
        Hostile = 0x01,
        Neutral = 0x04
    }

    [CreateAssetMenu(menuName = "Configuration/Models/Create Ability")]
    public class AbilityModel : BaseModel
    {
        [SerializeField]
        private AbilityType _abilityType = AbilityType.Target;
        [SerializeField]
        private AbilityUsage _abilityUsage;
        [SerializeField, Min(0f)]
        private float _cooldown;
        [SerializeField]
        private Sprite _icon;
        [Space, SerializeField, ObjectPicker]
        private string _effectPrefab;

        [Space, SerializeField]
        private VirtuesInfluenceBuilder _virtuesInfluencer;

        public AbilityType AbilityType => _abilityType;
        public AbilityUsage AbilityUsage => _abilityUsage;
        public float Cooldown => _cooldown;
        public Sprite Icon => _icon;
        public string EffectPrefab => _effectPrefab;
        public VirtuesInfluenceBuilder VirtuesInfluencer => _virtuesInfluencer;
    }
}