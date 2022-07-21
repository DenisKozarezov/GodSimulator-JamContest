using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Virtue Model")]
    public class VirtueModel : BaseModel
    {
        [SerializeField]
        private Sprite _icon;
        [Space, SerializeField]
        private List<VirtueActionModel> _virtueActions;

        public Sprite Icon => _icon;
        public IReadOnlyCollection<VirtueActionModel> VirtueActions => _virtueActions;
    }
}
