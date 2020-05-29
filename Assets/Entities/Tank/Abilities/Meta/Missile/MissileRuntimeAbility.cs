using Tanks.Pool;
using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class MissileRuntimeAbility : IRuntimeAbility
    {
        private readonly IPool<Missile> _missilePool;
        private readonly MissileAbility _missileAbilityMetaInfo;

        public MissileRuntimeAbility(IPool<Missile> missilePool, MissileAbility missileAbilityMetaInfo)
        {
            _missilePool = missilePool;
            _missileAbilityMetaInfo = missileAbilityMetaInfo;
        }
        
        public void Fire(in AbilityContext context)
        {
            var missile = _missilePool.Get();
            var transform = missile.transform;
            transform.position = context.TowerEnd.position;
            transform.rotation = context.TowerEnd.rotation;
            // TODO Pool
            // var missile = Instantiate(MissilePrefab, context.TowerEnd.position, context.TowerEnd.rotation);
            var direction = _missileAbilityMetaInfo.ShotRotationConvered * Vector3.forward;
            var force = context.RootTransform.TransformVector(direction) * _missileAbilityMetaInfo.ForceImpulse;
            // Debug.DrawRay(context.TowerEnd.position, direction, Color.blue, 10);
            missile.Fire(force, _missileAbilityMetaInfo.TargetsMask);
        }
    }
}