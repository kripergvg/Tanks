using System;
using UnityEngine;

namespace Tanks.Tank.Abilities
{
    [CreateAssetMenu(menuName = "Create Missile")]
    public class MissileAbility : Ability
    {
        public Missile MissilePrefab;
        public int ForceImpulse = 5;
        public Vector3 ShotRotation;

        private Quaternion ShotRotationConvered;

        public override void Fire(in AbilityContext context)
        {
            // TODO Pool
            var missile = Instantiate(MissilePrefab, context.TowerEnd.position, context.TowerEnd.rotation);
            var direction = ShotRotationConvered * Vector3.forward;
            var force= context.RootTransform.TransformVector(direction) * ForceImpulse;
            // Debug.DrawRay(context.TowerEnd.position, direction, Color.blue, 10);
            missile.Fire(force, TargetsMask);
        }

        private void OnEnable()
        {
            ShotRotationConvered = Quaternion.Euler(ShotRotation);
        }
    }
}