using System;
using System.Collections.Generic;
using Tanks.Mobs.Brain.FSMBrain;
using UnityEngine;

namespace Tanks.Mobs
{
    public class EntitiesSpawner : IEntityLocator, IEntityLifeManager
    {
        private readonly List<ITargetLocator> _targetLocators;
        private readonly Dictionary<IEntity, EntityInfo> _entitiesWithInfo = new Dictionary<IEntity, EntityInfo>();
        private readonly List<IEntity> _entitiesForLocator = new List<IEntity>();
        
        private readonly EntityDeps _entityDeps;

        public EntitiesSpawner(List<ITargetLocator> targetLocators)
        {
            _targetLocators = targetLocators;
            _entityDeps = new EntityDeps(this, new MainCameraStorage());
        }

        public List<IEntity> GetEntities()
        {
            return _entitiesForLocator;
        }

        public TEntity Spawn<TEntityFactory, TEntityRemover, TEntity>(TEntityFactory entityFactory,
            TEntityRemover entityRemover,
            Vector3 position,
            Quaternion rotation,
            Action onDespawn = null)
            where TEntityFactory : IEntityFactory<TEntity>
            where TEntityRemover : IEntityRemover
            where TEntity : IEntity
        {
            var entity = entityFactory.Create(in _entityDeps);
            entity.Position = position;
            entity.Rotation = rotation;
            entity.OnSpawn();
            
            foreach (var targetLocator in _targetLocators)
            {
                targetLocator.TryAddTarget(entity);
            }
            
            _entitiesWithInfo.Add(entity, new EntityInfo(onDespawn, entityRemover));
            return entity;
        }

        public void DeSpawn(IEntity entity)
        {
            foreach (var targetLocator in _targetLocators)
            {
                targetLocator.TryRemoveTarget(entity);
            }
            
            entity.BeforeDespawnEntity();
            var entityInfo = _entitiesWithInfo[entity];
            entityInfo.OnDespawn?.Invoke();
            entityInfo.Remover.Remove(entity);
            _entitiesWithInfo.Remove(entity);
        }
        
        private readonly struct EntityInfo
        {
            public EntityInfo(Action onDespawn, IEntityRemover remover)
            {
                OnDespawn = onDespawn;
                Remover = remover;
            }

            public Action OnDespawn { get; }
            public IEntityRemover Remover { get; }
        }
    }
}