using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Mobs
{
    public class MobSpawner<TPrefabInstantiator> 
        where TPrefabInstantiator : IEntityFactory<IEntity>
    {
        private readonly IRandom _random;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly MobSpawnerSettings _settings;
        private readonly List<TPrefabInstantiator> _mobInstantiators;

        public MobSpawner(IRandom random, IEntityLifeManager entityLifeManager, MobSpawnerSettings settings, List<TPrefabInstantiator> mobInstantiators)
        {
            _random = random;
            _entityLifeManager = entityLifeManager;
            _settings = settings;
            _mobInstantiators = mobInstantiators;
        }

        public void Start()
        {
            for (int i = 0; i < _settings.AliveCount; i++)
            {
                SpawnNew();
            }
        }

        private void SpawnNew()
        {
            var spawnPointIndex = _random.Next(_settings.SpawnPoints.Length);
            var spawnPoint = _settings.SpawnPoints[spawnPointIndex];

            var instantiatorIndex = _random.Next(_mobInstantiators.Count);
            var instantiator = _mobInstantiators[instantiatorIndex];

            _entityLifeManager.Spawn<TPrefabInstantiator, IEntity>(instantiator, spawnPoint, Quaternion.identity, OnDespawn);
        }

        private void OnDespawn()
        {
            SpawnNew();
        }
    }
}