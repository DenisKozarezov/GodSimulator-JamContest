using System;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Virtue Action Model")]
    public class VirtueActionModel : ScriptableObject, IEquatable<VirtueActionModel>
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

        public bool Equals(VirtueActionModel other)
        {
            return _id == other._id;
        }
    }
}
