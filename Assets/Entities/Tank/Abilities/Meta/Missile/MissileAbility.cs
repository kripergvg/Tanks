using UnityEngine;

namespace Tanks.Tank.Abilities
{
    [CreateAssetMenu(menuName = "Create Missile")]
    public class MissileAbility : Ability
    {
        public Missile MissilePrefab;
        public int ForceImpulse = 5;
        public Vector3 ShotRotation;

        public Quaternion ShotRotationConvered;

        private void OnEnable()
        {
            ShotRotationConvered = Quaternion.Euler(ShotRotation);
        }
    }
}