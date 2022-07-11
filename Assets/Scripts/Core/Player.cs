using System;
using System.Collections.Generic;
using UnityEngine;
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

    public class Player : IInitializable
    {
        [SerializeField]
        private int _prana;
        private Dictionary<VirtueModel, VirtueState> _virtuesLevels = new Dictionary<VirtueModel, VirtueState>();

        private readonly SignalBus _signalBus;
        public int Prana => _prana;

        public Player(SignalBus signalBus, PlayerSettings _playerSettings)
        {
            _signalBus = signalBus;
            foreach (var virtue in _playerSettings.Virtues)
            {
                _virtuesLevels.Add(virtue, new VirtueState());
            }
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<IPlayerUsedAbility>(OnAbilityCasted);

            ResetVirtues();
            foreach (var virtue in _virtuesLevels.Keys)
            {
                AddVirtueValue(virtue, (byte)UnityEngine.Random.Range(10, 90));
            }
        }
        private void OnAbilityCasted(IPlayerUsedAbility ability)
        {
            AddVirtueValue(ability.Ability.Virtue, ability.Ability.VirtueCost);
        }

        public void AddVirtueValue(VirtueModel virtue, byte value)
        {
            if (_virtuesLevels.TryGetValue(virtue, out VirtueState state))
            {
                state.Percent = (byte)Math.Min(state.Percent + value, Constants.VirtueValueMax);
                _signalBus.Fire(new PlayerVirtueChangedSignal { Virtue = virtue, State = state });
            }
        }
        public void RemoveVirtueValue(VirtueModel virtue, byte value)
        {
            if (_virtuesLevels.TryGetValue(virtue, out VirtueState state))
            {
                state.Percent = (byte)Math.Max(state.Percent - value, 0);
                _signalBus.Fire(new PlayerVirtueChangedSignal { Virtue = virtue, State = state });
            }
        }
        public void ResetVirtues()
        {
            foreach (var virtue in _virtuesLevels)
            {
                RemoveVirtueValue(virtue.Key, virtue.Value.Percent);
            }
        }
        public void AddPrana(int value)
        {
            _prana = Math.Min(_prana + value, Constants.PranaValueMax);
        }
        public void RemovePrana(int value)
        {
            _prana = Math.Max(_prana - value, 0);
        }
    }
}
