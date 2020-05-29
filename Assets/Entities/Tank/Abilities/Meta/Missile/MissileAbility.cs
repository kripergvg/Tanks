using UnityEngine;

namespace Tanks.Tank.Abilities.Missile
{
    [CreateAssetMenu(menuName = "Create Missile")]
    public class MissileAbility : Ability
    {
        [SerializeField]
        public int ForceImpulse = 5;
        [SerializeField]
        public Vector3 ShotRotation;
        [SerializeField]
        public Quaternion ShotRotationConvered;

        private void OnEnable()
        {
            ShotRotationConvered = Quaternion.Euler(ShotRotation);
        }
    }
}