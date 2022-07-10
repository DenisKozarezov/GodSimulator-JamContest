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
        private GameObject _abilityTooltip;

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
            InitializeAbilities();
        }
        private void InitializeAbilities()
        {
            foreach (var ability in _abilities)
            {
                var prefab = Instantiate(_abilitySlotPrefab, _abilitiesParent);
                var view = prefab.GetComponentInChildren<AbilityView>();
                var rectTransform = prefab.GetComponentInChildren<RectTransform>();
                view.SetAbility(ability);
                view.Execute += () => OnAbilityCasted(ability);
                view.MouseEnter += () => OnAbilityMouseEnter(rectTransform, ability);
                view.MouseExit += OnAbilityMouseExit;
            }
        }
        private void OnAbilityCasted(AbilityModel ability)
        {
            switch (ability.AbilityType)
            {
                case AbilityType.Target:
                    _signalBus.AbstractFire(new PlayerUsedTargetAbilitySignal { Ability = ability });
                    break;
                case AbilityType.NonTarget:
                    _signalBus.AbstractFire(new PlayerUsedNonTargetAbilitySignal { Ability = ability });
                    break;
                case AbilityType.Area:
                    _signalBus.AbstractFire(new PlayerUsedAreaAbilitySignal { Ability = ability });
                    break;
            }
        }
        private void OnAbilityMouseEnter(RectTransform abilityRect, AbilityModel ability)
        {
            if (_abilityTooltip == null)
            {
                RectTransform tooltip = CreateTooltip(ability);
                tooltip.position = Utils.GetCorrectedPosition(abilityRect, tooltip);
            }
        }
        private void OnAbilityMouseExit()
        {
            if (_abilityTooltip != null)
            {
                Destroy(_abilityTooltip.gameObject);
            }
        }
        private RectTransform CreateTooltip(AbilityModel ability)
        {
            _abilityTooltip = Instantiate(_abilityTooltipPrefab);
            var tooltip = _abilityTooltip.GetComponentInChildren<AbilityTooltip>();
            tooltip.Name = ability.DisplayName;
            tooltip.Cooldown = ability.Cooldown;
            tooltip.Description = ability.Description;
            tooltip.VirtueIcon.sprite = ability.Virtue.Icon;
            tooltip.AbilityType = ability.AbilityType;
            return tooltip.GetComponentInChildren<RectTransform>();
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