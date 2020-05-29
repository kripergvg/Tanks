using UnityEngine;

namespace Tanks.Mobs
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        private HealthSystem _healthSystem;

        public int MaxHealth = 100;
        public float Armour = 0.2f;
        
        public void Init(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
        }

        public abstract EntityType EntityType { get; }
        
        int IEntity.MaxHealth => MaxHealth;

        float IEntity.Armour => Armour;
        
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