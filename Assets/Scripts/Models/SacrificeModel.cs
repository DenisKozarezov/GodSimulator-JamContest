using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Sacrifice Model")]
    public class SacrificeModel : BaseModel
    {
        [Space, SerializeField]
        private Sprite _icon;

        [Header("Sacrifice")]
        [SerializeField]
        private VirtueModel _virtue;
        [SerializeField, Min(0)]
        private int _virtueBoost;

        public Sprite Icon => _icon;
        private VirtueModel Virtue => _virtue;
        private int VirtueBoost => _virtueBoost;
    }
}