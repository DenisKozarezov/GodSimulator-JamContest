using Core.Models;
using UnityEngine;

namespace Core
{
    public class Temple : MonoBehaviour
    {
        [SerializeField]
        private VirtueModel _virtue;
        [SerializeField]
        private byte _maxCapacityOfPriests;
        [SerializeField]
        private float _range;

        public VirtueModel Virtue => _virtue;
        public byte MaxCapacityOfPriests => _maxCapacityOfPriests;
        public float Range => _range;

        private void Start()
        {
            _range = 2f;
        }

        public void SetVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
        }
    }
}
