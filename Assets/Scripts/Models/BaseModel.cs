using System;
using UnityEngine;

namespace Core.Models
{
    public abstract class BaseModel : ScriptableObject, IEquatable<BaseModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField, TextArea]
        private string _description;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = this.name;
            }
        }

        public bool Equals(BaseModel other)
        {
            return _id == other._id;
        }
    }
}