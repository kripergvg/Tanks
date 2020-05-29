using Tanks.Mobs;
using Tanks.Tank.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tank
{
    public class TankFactory : IEntityFactory<TankViewModel>, IEntityRemover
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly TankViewModel _tankPrefab;
        private readonly Transform _abilitiesContainer;
        private readonly Slider _playerHealth;
        private readonly IRuntimeAbilityFactory _runtimeAbilityFactory;
        private readonly float _changeAbilityCooldown;
        private readonly InputManager _inputManager;

        public TankFactory(ITimeProvider timeProvider,
            IEntityLifeManager entityLifeManager, 
            TankViewModel tankPrefab,
            Transform abilitiesContainer,
            Slider playerHealth,
            IRuntimeAbilityFactory runtimeAbilityFactory,
            float changeAbilityCooldown,
            InputManager inputManager)
        {
            _timeProvider = timeProvider;
            _entityLifeManager = entityLifeManager;
            _tankPrefab = tankPrefab;
            _abilitiesContainer = abilitiesContainer;
            _playerHealth = playerHealth;
            _runtimeAbilityFactory = runtimeAbilityFactory;
            _changeAbilityCooldown = changeAbilityCooldown;
            _inputManager = inputManager;
        }

        public TankViewModel Create(in EntityDeps deps)
        {
            var tank = Object.Instantiate(_tankPrefab);
            var abilitySystem = new AbilitySystem(_timeProvider, _runtimeAbilityFactory, _changeAbilityCooldown);
            var healthSystem = new HealthSystem(_entityLifeManager, tank);
            tank.Init(healthSystem, abilitySystem, _abilitiesContainer, deps.MainCameraStorage, _playerHealth, _inputManager);
            return tank;
        }

        public void Remove(IEntity entity)
        {
            Object.Destroy(((TankViewModel)entity).gameObject);
        }
    }
}