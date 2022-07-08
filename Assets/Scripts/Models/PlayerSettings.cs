using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField]
        private AbilityModel[] _abilities;

        public IReadOnlyCollection<AbilityModel> Abilities => _abilities;
    }
}