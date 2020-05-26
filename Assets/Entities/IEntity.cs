using UnityEngine;

namespace Tanks.Mobs
{
    public interface IEntity
    {
        EntityType EntityType { get; }
        
        Vector3 Position { get; set; }

        Quaternion Rotation { get; set; }

        void OnDespawn();
    }
}