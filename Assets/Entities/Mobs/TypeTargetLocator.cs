using Tanks.Entities;
using Tanks.Mobs.Brain.FSMBrain;

namespace Tanks.Mobs
{
    public class TypeTargetLocator : ITargetLocator
    {
        private readonly EntityType _targetEntityType;
        private IEntity _entity;

        public TypeTargetLocator(EntityType targetEntityType)
        {
            _targetEntityType = targetEntityType;
        }

        public void TryAddTarget(IEntity entity)
        {
            if (entity.EntityType == _targetEntityType)
            {
                _entity = entity;
            }
        }

        public void TryRemoveTarget(IEntity entity)
        {
            if (entity == _entity)
            {
                _entity = null;
            }
        }

        public IEntity GetTarget()
        {
            return _entity;
        }
    }
}