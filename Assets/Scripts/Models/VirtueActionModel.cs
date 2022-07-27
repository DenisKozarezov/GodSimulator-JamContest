using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Virtue Action Model")]
    public class VirtueActionModel : BaseModel
    {
        [SerializeField]
        private byte _requiredVirtueLevel;

        public byte RequiredVirtueLevel => _requiredVirtueLevel;
    }
}
