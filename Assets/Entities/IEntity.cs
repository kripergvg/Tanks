using UnityEngine;

namespace Tanks.Entities
{
    public interface IEntity
    {
        EntityType EntityType { get; }
        
        Vector3 Position { get; set; }

        Quaternion Rotation { get; set; }
        
        int MaxHealth { get; }
        
        float Armour { get; }

        void OnSpawn();

        void TakeDamage(float damage);

        void BeforeDespawnEntity();
    }
}