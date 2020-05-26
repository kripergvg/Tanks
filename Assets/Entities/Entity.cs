using UnityEngine;

namespace Tanks.Mobs
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public abstract EntityType EntityType { get; }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public virtual void OnDespawn()
        {
        }
    }
}