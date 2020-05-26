using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Mobs
{
    public class EntitiesSpawner : IEntityLocator, IEntityLifeManager
    {
        private readonly IEntityDestroyer _destroyer;
        private readonly Dictionary<IEntity, EntityInfo> _entitiesWithInfo = new Dictionary<IEntity, EntityInfo>();
        private readonly List<IEntity> _entitiesForLocator = new List<IEntity>();
        
        private readonly EntityDeps _entityDeps;

        public EntitiesSpawner(IEntityDestroyer destroyer)
        {
            _destroyer = destroyer;
            _entityDeps = new EntityDeps(this, new MainCameraStorage());
        }

        public List<IEntity> GetEntities()
        {
            return _entitiesForLocator;
        }

        public TEntity Spawn<TEntityFactory, TEntity>(TEntityFactory entityFactory, Vector3 position, Quaternion rotation, Action onDespawn = null)
            where TEntityFactory : IEntityFactory<TEntity>
            where TEntity : IEntity
        {
            var entity = entityFactory.Create(in _entityDeps);
            entity.Position = position;
            entity.Rotation = rotation;
            _entitiesWithInfo.Add(entity, new EntityInfo(onDespawn));
            return entity;
        }

        public void DeSpawn(IEntity entity)
        {
            entity.OnDespawn();
            var entityInfo = _entitiesWithInfo[entity];
            entityInfo.OnDespawn?.Invoke();
            _entitiesWithInfo.Remove(entity);
            _destroyer.Destory(entity);
        }
        
        private readonly struct EntityInfo
        {
            public EntityInfo(Action onDespawn)
            {
                OnDespawn = onDespawn;
            }

            public Action OnDespawn { get; }
        }
    }
}