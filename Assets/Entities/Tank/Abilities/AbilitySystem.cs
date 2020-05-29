namespace Tanks.Tank.Abilities
{
    public class AbilitySystem
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IRuntimeAbilityFactory _runtimeAbilityFactory;
        private readonly float _changeAbilityCooldown;

        private AbilityInfo[] _abilities;
        private int _selectedAbility;
        private float? _lastTimeChangeAbility;

        public AbilitySystem(ITimeProvider timeProvider, IRuntimeAbilityFactory runtimeAbilityFactory, float changeAbilityCooldown)
        {
            _timeProvider = timeProvider;
            _runtimeAbilityFactory = runtimeAbilityFactory;
            _changeAbilityCooldown = changeAbilityCooldown;
        }

        public void Init<TPrefabInstantiator>(IAbility[] abilities, TPrefabInstantiator abilityUiInstantiator, in TankDeps tankDeps)
            where TPrefabInstantiator : IFactory<IAbilityUi>
        {
            _abilities = new AbilityInfo[abilities.Length];
            for (var abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                var ability = abilities[abilityIndex];
                var runtimeAbility = _runtimeAbilityFactory.Create(ability, tankDeps);
                var abilityUi = abilityUiInstantiator.Create();
                abilityUi.Init(ability.Icon, ability.Name, ability.Cooldown, ability.ShowCooldown);

                _abilities[abilityIndex] = new AbilityInfo(ability, abilityUi, runtimeAbility, null);
            }

            if (_abilities.Length > 0)
            {
                SetSelected(0);
            }
        }

        public void Fire(in AbilityContext context)
        {
            var newFireDate = _timeProvider.Time;
            var ability = _abilities[_selectedAbility];
            if (ability.LastFireDate == null 
                || newFireDate - ability.LastFireDate > ability.AbilityMetaInfo.Cooldown)
            {
                ability.RuntimeAbility.Fire(in context);
                ability.UiElement.SetFireDate(newFireDate);
                _abilities[_selectedAbility] = new AbilityInfo(ability, newFireDate);
            }
        }

        public void NextAbility()
        {
            if (CanChangeAbility())
            {
                var nextAbilityIndex = _selectedAbility + 1;
                if (nextAbilityIndex == _abilities.Length)
                {
                    nextAbilityIndex = 0;
                }
            
                ChangeAbility(nextAbilityIndex);
            }
        }

        public void PreviousAbility()
        {
            if (CanChangeAbility())
            {
                var previousAbilityIndex = _selectedAbility - 1;
                if (previousAbilityIndex == -1)
                {
                    previousAbilityIndex = _abilities.Length - 1;
                }

                ChangeAbility(previousAbilityIndex);
            }
        }

        private bool CanChangeAbility()
        {
            return _lastTimeChangeAbility == null
                   || _timeProvider.Time - _lastTimeChangeAbility > _changeAbilityCooldown;
        }

        private void ChangeAbility(int newAbilityIndex)
        {
            _abilities[_selectedAbility].UiElement.RemoveSelected();
            SetSelected(newAbilityIndex);
            _lastTimeChangeAbility = _timeProvider.Time;
        }

        private void SetSelected(int newAbilityIndex)
        {
            _abilities[newAbilityIndex].UiElement.SetSelected();
            _selectedAbility = newAbilityIndex;
        }

        private readonly struct AbilityInfo
        {
            public AbilityInfo(AbilityInfo abilityInfo, float fireDate)
                : this(abilityInfo.AbilityMetaInfo, abilityInfo.UiElement, abilityInfo.RuntimeAbility, fireDate)
            {
            }

            public AbilityInfo(IAbility abilityMetaInfo, IAbilityUi uiElement, IRuntimeAbility runtimeAbility,  float? lastFireDate)
            {
                AbilityMetaInfo = abilityMetaInfo;
                UiElement = uiElement;
                RuntimeAbility = runtimeAbility;
                LastFireDate = lastFireDate;
            }

            public IAbility AbilityMetaInfo { get; }
            public IAbilityUi UiElement { get; }
            public IRuntimeAbility RuntimeAbility { get; }
            public float? LastFireDate { get; }
        }
    }
}