using System;
using UnityEngine;

namespace Tanks.Mobs
{
    public interface IEntityLifeManager
    {
        TEntity Spawn<TEntityFactory, TEntity>(TEntityFactory entityFactory, Vector3 position, Quaternion rotation, Action onDespawn=null)
            where TEntityFactory : IEntityFactory<TEntity>
            where TEntity : IEntity;

        void DeSpawn(IEntity entity);
    }
}