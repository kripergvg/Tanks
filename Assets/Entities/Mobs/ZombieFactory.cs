using Tanks.DI;
using Tanks.FSM;
using Tanks.Mobs.Brain.FSMBrain;
using Tanks.Pool;

namespace Tanks.Mobs
{
    public class ZombieFactory : IEntityFactory<IEntity>, IEntityRemover
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly ITargetLocator _targetLocator;
        private readonly StateMachineFactory _stateMachineFactory;
        private readonly IDoor[] _doors;
        private readonly IPool<Zombie> _pool;

        public ZombieFactory(ITimeProvider timeProvider,
            IEntityLifeManager entityLifeManager,
            ITargetLocator targetLocator,
            StateMachineFactory stateMachineFactory,
            IDoor[] doors,
            IPool<Zombie> pool)
        {
            _timeProvider = timeProvider;
            _entityLifeManager = entityLifeManager;
            _targetLocator = targetLocator;
            _stateMachineFactory = stateMachineFactory;
            _doors = doors;
            _pool = pool;
        }

        public IEntity Create(in EntityDeps deps)
        {
            // var zombie = Object.Instantiate(_prefab);
            var zombie = _pool.Get();
            var healthSystem = new HealthSystem(_entityLifeManager, zombie);

            zombie.Init(_timeProvider, healthSystem, deps.MainCameraStorage, _targetLocator, _stateMachineFactory, _doors);
            return zombie;
        }

        public void Remove(IEntity entity)
        {
            _pool.Return((Zombie)entity);
        }
    }
}