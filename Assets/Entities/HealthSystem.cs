using System;

namespace Tanks.Mobs
{
    public class HealthSystem
    {
        private readonly float _armour;
        private readonly IEntityLifeManager _entityLifeManager;
        private readonly IEntity _entity;
        private float _health;
        private readonly int _maxHealth;

        public HealthSystem(IEntityLifeManager entityLifeManager, IEntity entity)
        {
            _entityLifeManager = entityLifeManager;
            _entity = entity;
            _health = entity.MaxHealth;
            _maxHealth = entity.MaxHealth;
            _armour = entity.Armour;
        }
        
        public void TakeDamage(float damage)
        {
            var damageReduction = damage * _armour;
            var damageAfterReduction = damage - damageReduction;
            _health = Math.Max(0, _health - damageAfterReduction);
            if (_health < 0.01)
            {
                _entityLifeManager.DeSpawn(_entity);
            }
        }

        public float GetHealthInPercent()
        {
            return _health / _maxHealth;
        }
    }
}