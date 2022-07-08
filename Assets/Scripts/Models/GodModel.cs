using System;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create God Model")]
    public class GodModel : ScriptableObject, IEquatable<GodModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField, TextArea]
        private string _description;
        [SerializeField]
        private Color _color;
        [SerializeField]
        private Sprite _portrait;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Color Color => _color;
        public Sprite Portrait => _portrait;

        public bool Equals(GodModel other)
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