using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create God Model")]
    public class GodModel : BaseModel
    {
        [SerializeField]
        private Color _color;
        [SerializeField]
        private Sprite _portrait;

        public Color Color => _color;
        public Sprite Portrait => _portrait;

        public static implicit operator uint(GodModel god)
        {
            return god.ID;
        }
    }
}