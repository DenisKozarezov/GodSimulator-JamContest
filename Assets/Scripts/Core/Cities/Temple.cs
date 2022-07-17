using UnityEngine;
using Core.Models;

namespace Core.Cities
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

        public void SetInitialValues(VirtueModel virtue, byte maxCapacityOfPriests, float range)
        {
            _virtue = virtue;
            _maxCapacityOfPriests = maxCapacityOfPriests;
            _range = range;
        }

        public void ChangeVirtue(VirtueModel virtue)
        {
            _virtue = virtue;
        }

        private void Start()
        {
            _range = GetTempleRange(1);
        }

        private float GetTempleRange(byte virtueLevel)
        {
            switch (virtueLevel)
            {
                case 1:
                    return 3f;
                case 2:
                    return 5f;
                default:
                    return 10f;
            }
        }
    }
}
