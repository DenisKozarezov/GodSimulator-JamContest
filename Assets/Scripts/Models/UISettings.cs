using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create UI Settings")]
    public class UISettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Range(0f, 15f)]
        private float _outlineWidth;

        public float OutlineWidth => _outlineWidth;
    }
}