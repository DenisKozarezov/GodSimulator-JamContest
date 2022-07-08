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
        [SerializeField]
        private GameObject _abilityTooltipPrefab;

        private IReadOnlyCollection<AbilityModel> _abilities;
        private SignalBus _signalBus;
        private AbilityTooltip _abilityTooltip;
        private Canvas _canvas;

        [Inject]
        public void Construct(PlayerSettings playerSettings, SignalBus signalBus, Canvas canvas)
        {
            _abilities = playerSettings.Abilities;
            _signalBus = signalBus;
            _canvas = canvas;
        }

        private void Awake()
        {
            Clear();
        }
        private void Start()
        {
            InitializeAbilities();
        }
        private void InitializeAbilities()
        {
            foreach (var ability in _abilities)
            {
                var prefab = Instantiate(_abilitySlotPrefab, _abilitiesParent);
                var view = prefab.GetComponentInChildren<AbilityView>();
                view.SetAbility(ability);
                view.Execute += () => OnAbilityCasted(ability);
                view.MouseEnter += () => OnAbilityMouseEnter(ability);
                view.MouseExit += OnAbilityMouseExit;
            }
        }
        private void OnAbilityCasted(AbilityModel ability)
        {
            _signalBus.Fire(new PlayerUsedAbilitySignal { Ability = ability });
        }
        private void OnAbilityMouseEnter(AbilityModel ability)
        {
            if (_abilityTooltip == null)
            {
                _abilityTooltip = Instantiate(_abilityTooltipPrefab, _canvas.transform).GetComponent<AbilityTooltip>();
                _abilityTooltip.Name = ability.DisplayName;
                _abilityTooltip.Cooldown = ability.Cooldown;
                _abilityTooltip.Description = ability.Description;
            }
        }
        private void OnAbilityMouseExit()
        {
            if (_abilityTooltip != null)
            {
                Destroy(_abilityTooltip.gameObject);
            }
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