using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Mobs
{
    public class MobSpawner<TEntityFactory> 
        where TEntityFactory : IEntityFactory<IEntity>, IEntityRemover
    {
        private readonly IRandom _random;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly MobSpawnerSettings _settings;
        private readonly List<TEntityFactory> _mobFactories;

        public MobSpawner(IRandom random, IEntityLifeManager entityLifeManager, MobSpawnerSettings settings, List<TEntityFactory> mobFactories)
        {
            _random = random;
            _entityLifeManager = entityLifeManager;
            _settings = settings;
            _mobFactories = mobFactories;
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

            var mobFactoryIndex = _random.Next(_mobFactories.Count);
            var mobFactory = _mobFactories[mobFactoryIndex];

            _entityLifeManager.Spawn<TEntityFactory, TEntityFactory, IEntity>(mobFactory, mobFactory, spawnPoint, Quaternion.identity, OnDespawn);
        }

        private void OnDespawn()
        {
            SpawnNew();
        }
    }
}