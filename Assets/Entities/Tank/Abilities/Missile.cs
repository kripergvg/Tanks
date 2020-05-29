using System.Buffers;
using System.Collections;
using Tanks.Mobs;
using Tanks.Pool;
using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class Missile : MonoBehaviour, IPoolable
    {
        public Rigidbody Body;
        public float ExplosionRadius = 4f;
        public float Damage = 40;
        public float MaxLiveTime = 5;

        private IPool<Missile> _poolOwner;
        private LayerMask _entityLayerMask;
        
        private bool _collided;
        private bool _returnedToPool;

        public void SetPoolOwner(IPool<Missile> poolOwner)
        {
            _poolOwner = poolOwner;
        }
        
        public void PoolInit()
        {
            _returnedToPool = false;
            gameObject.SetActive(true);
        }

        public void Fire(Vector3 force, LayerMask layerMask)
        {
            _entityLayerMask = layerMask;
            // TODO Research
            Body.AddForce(force, ForceMode.Impulse);

            StartCoroutine(CheckReturnedToPool());
        }

        public void PoolClear()
        {
            Body.velocity = Vector3.zero;
            Body.angularVelocity = Vector3.zero;
            _collided = false;
            gameObject.SetActive(false);
        }

        private IEnumerator CheckReturnedToPool()
        {
            yield return new WaitForSeconds(MaxLiveTime);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_collided)
            {
                var collidersBuffer = ArrayPool<Collider>.Shared.Rent(100);
                try
                {
                    var collideCount = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, collidersBuffer, _entityLayerMask);
                    for (int i = 0; i < collideCount; i++)
                    {
                        var targetCollider = collidersBuffer[i];
                        var entity = targetCollider.GetComponent<IEntity>();

                        var distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                        var damageMultiplier = distance / ExplosionRadius;
                        entity.TakeDamage(Damage * damageMultiplier);
                    }

                    _collided = true;
                    ReturnToPool();
                }
                finally
                {
                    ArrayPool<Collider>.Shared.Return(collidersBuffer);
                }
            }
        }

        private void ReturnToPool()
        {
            if (!_returnedToPool)
            {
                _returnedToPool = true;
                _poolOwner.Return(this);
            }
        }
    }
}