using UnityEngine;
using Editor;

namespace Core.UI
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField, ObjectPicker]
        private string _abilitySlotPrefab;
    }
}