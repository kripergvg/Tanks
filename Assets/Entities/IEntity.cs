using UnityEngine;

namespace Tanks.Mobs
{
    public interface IEntity
    {
        EntityType EntityType { get; }
        
        Vector3 Position { get; set; }

        Quaternion Rotation { get; set; }
        
        int MaxHealth { get; }
        
        float Armour { get; }

        void TakeDamage(float damage);

        void BeforeDestroyEntity();

        void DestroyEntity();

    }
}