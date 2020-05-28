using System.Buffers;
using Tanks.Mobs;
using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class Missile : MonoBehaviour
    {
        public Rigidbody Body;
        public float ExplosionRadius = 4f;
        public float Damage = 40;
        public float MaxLiveTime = 5;

        private bool _collided;
        private LayerMask _layerMask;

        public void Fire(Vector3 force, LayerMask layerMask)
        {
            _layerMask = layerMask;
            // TODO Research
            Body.AddForce(force, ForceMode.Impulse);
            // TODO Переделать на пул
            Destroy(gameObject, MaxLiveTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_collided)
            {
                var collidersBuffer = ArrayPool<Collider>.Shared.Rent(100);
                var collideCount = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, collidersBuffer, _layerMask);
                for (int i = 0; i < collideCount; i++)
                {
                    var targetCollider = collidersBuffer[i];
                    var entity = targetCollider.GetComponent<IEntity>();

                    var distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                    var damageMultiplier = distance / ExplosionRadius;
                    entity.TakeDamage(Damage * damageMultiplier);
                }

                Destroy(gameObject);
                _collided = true;
                
                ArrayPool<Collider>.Shared.Return(collidersBuffer);
            }
        }
    }
}