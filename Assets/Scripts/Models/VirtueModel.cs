using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Virtue Model")]
    public class VirtueModel : ScriptableObject
    {
        [SerializeField]
        private uint _id;
        [SerializeField]
        private string _displayName;
        [SerializeField]
        private List<VirtueActionModel> _virtuesActions;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public IReadOnlyCollection<VirtueActionModel> Abilities => _virtuesActions;
    }
}
