using Tanks.DI;
using Tanks.Mobs;
using Tanks.Tank.Abilities;
using UnityEngine;

namespace Tanks.Tank
{
    public class TankFactory : IEntityFactory<TankViewModel>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly TankViewModel _tankPrefab;
        private readonly Transform _abilitiesContainer;

        public TankFactory(ITimeProvider timeProvider,
            IEntityLifeManager entityLifeManager, 
            TankViewModel tankPrefab,
            Transform abilitiesContainer)
        {
            _timeProvider = timeProvider;
            _entityLifeManager = entityLifeManager;
            _tankPrefab = tankPrefab;
            _abilitiesContainer = abilitiesContainer;
        }

        public TankViewModel Create(in EntityDeps deps)
        {
            var tank = Object.Instantiate(_tankPrefab);
            var abilitySystem = new AbilitySystem(_timeProvider);
            var healthSystem = new HealthSystem(_entityLifeManager, tank);
            tank.Init(healthSystem, abilitySystem, _abilitiesContainer, deps.MainCameraStorage);
            return tank;
        }
    }
}