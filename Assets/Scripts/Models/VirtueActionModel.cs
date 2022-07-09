using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Virtue Action Model")]
    public class VirtueActionModel : ScriptableObject
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
    }
}
