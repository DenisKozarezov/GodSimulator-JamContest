using UnityEngine;
using TMPro;

namespace Core.UI
{
    public class MovingPriestsIcon : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _amount;

        public void SetAmount(ushort amount)
        {
            _amount.text = amount.ToString();
        }
    }
}