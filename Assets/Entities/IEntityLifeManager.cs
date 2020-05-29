using System;
using Tanks.Mobs;
using UnityEngine;

namespace Tanks.Entities
{
    public interface IEntityLifeManager
    {
        TEntity Spawn<TEntityFactory, TEntityRemover, TEntity>(TEntityFactory entityFactory,
            TEntityRemover entityRemover,
            Vector3 position,
            Quaternion rotation,
            Action onDespawn = null)
            where TEntityFactory : IEntityFactory<TEntity>
            where TEntityRemover : IEntityRemover
            where TEntity : IEntity;
        
        void DeSpawn(IEntity entity);
    }
}