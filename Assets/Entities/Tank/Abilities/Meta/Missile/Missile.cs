using System.Buffers;
using System.Collections;
using Tanks.Entities;
using Tanks.Pool;
using UnityEngine;

namespace Tanks.Tank.Abilities.Missile
{
    public class Missile : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private Rigidbody _body;
        [SerializeField]
        private float _explosionRadius = 4f;
        [SerializeField]
        private float _damage = 40;
        [SerializeField]
        private float _maxLiveTime = 5;

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
            _body.AddForce(force, ForceMode.Impulse);

            StartCoroutine(CheckReturnedToPool());
        }

        public void PoolClear()
        {
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _collided = false;
            gameObject.SetActive(false);
        }

        private IEnumerator CheckReturnedToPool()
        {
            yield return new WaitForSeconds(_maxLiveTime);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_collided)
            {
                var collidersBuffer = ArrayPool<Collider>.Shared.Rent(100);
                try
                {
                    var collideCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, collidersBuffer, _entityLayerMask);
                    for (int i = 0; i < collideCount; i++)
                    {
                        var targetCollider = collidersBuffer[i];
                        var entity = targetCollider.GetComponent<IEntity>();

                        var distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                        var damageReductionRate = distance / _explosionRadius;
                        var damageReduction = _damage * damageReductionRate;
                        entity.TakeDamage(_damage - damageReduction);
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