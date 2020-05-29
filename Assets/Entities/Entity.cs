using UnityEngine;

namespace Tanks.Mobs
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        private HealthSystem _healthSystem;

        [SerializeField]
        private int _maxHealth = 100;
        [SerializeField]
        private float _armour = 0.2f;
        
        public void Init(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
        }

        public abstract EntityType EntityType { get; }
        
        int IEntity.MaxHealth => _maxHealth;

        float IEntity.Armour => _armour;
        
        public virtual Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public virtual void OnSpawn()
        {
        }

        public void TakeDamage(float damage)
        { 
            _healthSystem.TakeDamage(damage);
        }

        public virtual void BeforeDespawnEntity()
        {
        }
    }
}