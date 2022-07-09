using Core.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    struct VirtueState
    {
        public byte Level;
        public byte Percent;
    }

    public class Player
    {
        [SerializeField]
        private int _prana;
        private Dictionary<VirtueModel, VirtueState> _virtuesLevels;

        public int Prana => _prana;

        private void Awake()
        {
            _virtuesLevels = new Dictionary<VirtueModel, VirtueState>();
        }
    }
}
