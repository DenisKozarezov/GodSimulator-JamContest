using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Music Box")]
    public class MusicBox : ScriptableObject
    {
        enum OrderType : byte
        {
            InOrder = 0x00,
            Random = 0x01
        }

        [Header("Music")]
        [SerializeField]
        private OrderType _playingOrder;
        [SerializeField]
        private List<AudioClip> OST;

        private int _currentIndex;

        public AudioClip GetClip()
        {
            switch (_playingOrder)
            {
                case OrderType.Random:
                    _currentIndex = Random.Range(0, OST.Count - 1);
                    break;
                case OrderType.InOrder:
                    _currentIndex++;
                    break;
            }
            return OST[_currentIndex];
        }
    }
}