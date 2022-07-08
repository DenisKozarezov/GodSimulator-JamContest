using System;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create City")]
    public class CityModel : ScriptableObject, IEquatable<CityModel>
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;

        public uint ID => _id;
        public string DisplayName => _displayName;

        public bool Equals(CityModel other)
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