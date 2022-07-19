using System;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Sacrifice Model")]
    public class SacrificeModel : ScriptableObject, IEquatable<SacrificeModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField, TextArea]
        private string _description;

        [Header("Sacrifice")]
        [SerializeField]
        private VirtueModel _virtue;
        [SerializeField, Min(0)]
        private int _virtueBoost;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        private VirtueModel Virtue => _virtue;
        private int VirtueBoost => _virtueBoost;

        public bool Equals(SacrificeModel other)
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