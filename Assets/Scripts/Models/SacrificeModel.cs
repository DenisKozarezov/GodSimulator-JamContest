using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Models
{
    [Serializable]
    public struct VirtuesInfluenceBuilder
    {
        [Serializable]
        public struct InternalStruct
        {
            public VirtueModel Virtue;
            public byte Value;
        }
        [SerializeField]
        private InternalStruct[] _buffedVirtues;
        [SerializeField]
        private InternalStruct[] _debuffedVirtues;
        public IReadOnlyCollection<InternalStruct> BuffedVirtues => _buffedVirtues;
        public IReadOnlyCollection<InternalStruct> DebuffedVirtues => _debuffedVirtues;
    }

    [CreateAssetMenu(menuName = "Configuration/Models/Create Sacrifice Model")]
    public class SacrificeModel : BaseModel
    {
        [Space, SerializeField]
        private Sprite _icon;

        [Space, SerializeField]
        private VirtuesInfluenceBuilder _virtuesInfluencer;

        public Sprite Icon => _icon;
        private VirtuesInfluenceBuilder VirtuesInfluencer => _virtuesInfluencer;
    }
}