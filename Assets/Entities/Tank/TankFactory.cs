using Tanks.DI;
using Tanks.Mobs;
using Tanks.Tank.Abilities;
using UnityEngine;

namespace Tanks.Tank
{
    public class TankFactory : IEntityFactory<TankViewModel>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly TankViewModel _tankPrefab;
        private readonly Transform _abilitiesContainer;

        public TankFactory(ITimeProvider timeProvider, TankViewModel tankPrefab, Transform abilitiesContainer)
        {
            _timeProvider = timeProvider;
            _tankPrefab = tankPrefab;
            _abilitiesContainer = abilitiesContainer;
        }

        public TankViewModel Create(in EntityDeps deps)
        {
            var tank = Object.Instantiate(_tankPrefab);
            var abilitySystem = new AbilitySystem(_timeProvider);
            tank.Init(abilitySystem, _abilitiesContainer, deps.MainCameraStorage);
            return tank;
        }
    }
}