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
        private Sprite _icon;
        [Space, SerializeField]
        private List<VirtueActionModel> _virtueActions;

        public uint ID => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public IReadOnlyCollection<VirtueActionModel> VirtueActions => _virtueActions;
    }
}
