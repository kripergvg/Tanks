using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class AbilitySystem
    {
        private readonly ITimeProvider _timeProvider;

        private AbilityInfo[] _abilities;
        private int _selectedAbility;

        public AbilitySystem(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Init<TPrefabInstantiator>(IAbility[] abilities, TPrefabInstantiator abilityUiInstantiator)
            where TPrefabInstantiator : IFactory<IAbilityUi>
        {
            _abilities = new AbilityInfo[abilities.Length];
            for (var abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                var ability = abilities[abilityIndex];
                var abilityUi = abilityUiInstantiator.Create();
                abilityUi.Init(ability.Icon, ability.Name, ability.Cooldown);

                _abilities[abilityIndex] = new AbilityInfo(ability, abilityUi, null);
            }
        }

        public void Fire(ref AbilityContext context)
        {
            var newFireDate = _timeProvider.Time;
            var ability = _abilities[_selectedAbility];
            if (ability.LastFireDate == null 
                || newFireDate - ability.LastFireDate > ability.AbilityMetaInfo.Cooldown)
            {
                ability.AbilityMetaInfo.Fire(ref context);
                ability.UiElement.SetFireDate(newFireDate);
                _abilities[_selectedAbility] = new AbilityInfo(ability, newFireDate);
            }
        }

        public void NextAbility()
        {
            var nextAbilityIndex = _selectedAbility + 1;
            if (nextAbilityIndex == _abilities.Length)
            {
                nextAbilityIndex = 0;
            }
            
            ChangeAbility(nextAbilityIndex);
        }

        public void PreviousAbility()
        {
            var previousAbilityIndex = _selectedAbility - 1;
            if (previousAbilityIndex == -1)
            {
                previousAbilityIndex = _abilities.Length - 1;
            }

            ChangeAbility(previousAbilityIndex);
        }

        private void ChangeAbility(int newAbilityIndex)
        {
            _abilities[_selectedAbility].UiElement.RemoveSelected();
            _abilities[newAbilityIndex].UiElement.SetSelected();
            _selectedAbility = newAbilityIndex;
        }

        private readonly struct AbilityInfo
        {
            public AbilityInfo(AbilityInfo abilityInfo, float fireDate)
                : this(abilityInfo.AbilityMetaInfo, abilityInfo.UiElement, fireDate)
            {
            }

            public AbilityInfo(IAbility abilityMetaInfo, IAbilityUi uiElement, float? lastFireDate)
            {
                AbilityMetaInfo = abilityMetaInfo;
                UiElement = uiElement;
                LastFireDate = lastFireDate;
            }

            public IAbility AbilityMetaInfo { get; }
            public IAbilityUi UiElement { get; }
            public float? LastFireDate { get; }
        }
    }
}