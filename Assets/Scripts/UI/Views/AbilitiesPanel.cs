using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Models;
using Core.Infrastructure;

namespace Core.UI
{
    public class AbilitiesPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform _abilitiesParent;
        [SerializeField]
        private GameObject _abilitySlotPrefab;

        private IReadOnlyCollection<AbilityModel> _abilities;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _abilities = playerSettings.Abilities;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            Clear();
        }
        private void Start()
        {
            foreach (var ability in _abilities)
            {
                var prefab = Instantiate(_abilitySlotPrefab, _abilitiesParent);
                var view = prefab.GetComponentInChildren<AbilityView>();
                view.SetAbility(ability);
                view.Execute += () => OnAbilityCasted(ability);
            }
        }
        private void OnAbilityCasted(AbilityModel ability)
        {
            _signalBus.Fire(new PlayerUsedAbilitySignal { Ability = ability });
        }

        private void Clear()
        {
            for (int i = 0; i < _abilitiesParent.childCount; i++)
            {
                Destroy(_abilitiesParent.GetChild(i).gameObject);
            }
        }
    }
}