using System;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Ability")]
    public class AbilityModel : ScriptableObject, IEquatable<AbilityModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField, TextArea]
        private string _description;
        [SerializeField, Min(0f)]
        private float _cooldown;
        [Space, SerializeField]
        private Sprite _icon;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public float Cooldown => _cooldown;
        public Sprite Icon => _icon;

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