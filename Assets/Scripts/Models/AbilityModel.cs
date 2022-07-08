using System;
using UnityEngine;
using Editor;

namespace Core.Models
{
    public enum AbilityType : byte
    {
        NonTarget = 0x00,
        Target = 0x01,
        Area = 0x02,
    }

    [Flags]
    public enum AbilityUsage : byte
    {
        Friendly = 0x00,
        Hostile = 0x01,
        Neutral = 0x04
    }

    [CreateAssetMenu(menuName = "Configuration/Models/Create Ability")]
    public class AbilityModel : ScriptableObject, IEquatable<AbilityModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField, TextArea]
        private string _description;
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

        public uint ID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public AbilityType AbilityType => _abilityType;
        public AbilityUsage AbilityUsage => _abilityUsage;
        public float Cooldown => _cooldown;
        public Sprite Icon => _icon;
        public string EffectPrefab => _effectPrefab;

        public bool Equals(AbilityModel other)
        {
            return _id == other._id;
        }

        protected virtual void OnEnable()
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = this.name;
            }
        }
    }
}