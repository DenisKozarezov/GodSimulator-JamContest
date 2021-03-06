using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Zenject;
using Core.Models;
using Core.Infrastructure;

namespace Core
{
    public class VirtueState
    {
        public byte Level;
        public byte Percent;
    }

    public class Player : IInitializable, ILateDisposable, IEquatable<Player>
    {
        private int _prana;
        private Dictionary<VirtueModel, VirtueState> _virtuesLevels = new Dictionary<VirtueModel, VirtueState>();
        private float _faithRate = 1f;

        private SignalBus _signalBus;

        public uint ID;
        public int Prana => _prana;
        public float FaithRate => _faithRate;

        [Inject]
        public void Construct(SignalBus signalBus, PlayerSettings _playerSettings)
        {
            _signalBus = signalBus;
            foreach (var virtue in _playerSettings.Virtues)
            {
                _virtuesLevels.Add(virtue, new VirtueState());
            }
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<IPlayerCastedAbility>(OnAbilityCasted);

            ResetVirtues();
            foreach (var virtue in _virtuesLevels.Keys)
            {
                AddVirtueValue(virtue, (byte)UnityEngine.Random.Range(10, 90));
            }
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.Unsubscribe<IPlayerCastedAbility>(OnAbilityCasted);
        }
        private void OnAbilityCasted(IPlayerCastedAbility ability)
        {
            foreach (var virtue in ability.Ability.VirtuesInfluencer.BuffedVirtues)
            {
                AddVirtueValue(virtue.Virtue, virtue.Value);
            }
            foreach (var virtue in ability.Ability.VirtuesInfluencer.DebuffedVirtues)
            {
                ReduceVirtueValue(virtue.Virtue, virtue.Value);
            }
        }

        public void AddVirtueValue(VirtueModel virtue, byte value)
        {
            if (_virtuesLevels.TryGetValue(virtue, out VirtueState state))
            {
                state.Percent = (byte)math.min(state.Percent + value, Constants.VirtueValueMax);
                _signalBus.Fire(new PlayerVirtueChangedSignal { Virtue = virtue, State = state });
            }
        }
        public void ReduceVirtueValue(VirtueModel virtue, byte value)
        {
            if (_virtuesLevels.TryGetValue(virtue, out VirtueState state))
            {
                state.Percent = (byte)math.max(state.Percent - value, 0);
                _signalBus.Fire(new PlayerVirtueChangedSignal { Virtue = virtue, State = state });
            }
        }
        public void ResetVirtues()
        {
            foreach (var virtue in _virtuesLevels)
            {
                ReduceVirtueValue(virtue.Key, virtue.Value.Percent);
            }
        }
        public void AddPrana(int value)
        {
            _prana = math.min(_prana + value, Constants.PranaValueMax);
        }
        public void RemovePrana(int value)
        {
            _prana = math.max(_prana - value, 0);
        }
        public bool Equals(Player other)
        {
            if (other == null) return false;
            return ID == other.ID;
        }

        public static implicit operator uint(Player player)
        {
            return player.ID;
        }
    }
}
