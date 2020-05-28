using Tanks.FSM;
using Tanks.Mobs.Brain.FSMBrain;
using UnityEngine;

namespace Tanks.Mobs
{
    public class ZombieFactory : IEntityFactory<Zombie>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly Zombie _prefab;
        private readonly ITargetLocator _targetLocator;
        private readonly StateMachineFactory _stateMachineFactory;

        public ZombieFactory(ITimeProvider timeProvider,
            IEntityLifeManager entityLifeManager,
            Zombie prefab,
            ITargetLocator targetLocator,
            StateMachineFactory stateMachineFactory)
        {
            _timeProvider = timeProvider;
            _entityLifeManager = entityLifeManager;
            _prefab = prefab;
            _targetLocator = targetLocator;
            _stateMachineFactory = stateMachineFactory;
        }

        public Zombie Create(in EntityDeps deps)
        {
            var zombie = Object.Instantiate(_prefab);
            var healthSystem = new HealthSystem(_entityLifeManager, zombie);
            
            zombie.Init(_timeProvider, healthSystem, deps.MainCameraStorage, _targetLocator, _stateMachineFactory);
            return zombie;
        }
    }
}