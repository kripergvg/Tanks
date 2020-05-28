using Tanks.Mobs.Brain.FSMBrain;

namespace Tanks.Mobs
{
    public class SimpleAttacker : IAttacker
    {
        private readonly int _damage;
        private IEntity _attackTarget;

        public SimpleAttacker(int damage)
        {
            _damage = damage;
        }
        
        public void Attack(IEntity target)
        {
            _attackTarget = target;
        }
        
        public void OnEntityTriggerStay(IEntity entity)
        {
            CheckTrigger(entity);
        }

        private void CheckTrigger(IEntity entity)
        {
            if (_attackTarget == entity)
            {
                entity.TakeDamage(_damage);
                _attackTarget = null;
            }
        }

        public void OnEntityTriggerEnter(IEntity entity)
        {
            CheckTrigger(entity);
        }
    }
}