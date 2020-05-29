using UnityEngine;

namespace Tanks.Tank.Abilities.MachineGun
{
    [CreateAssetMenu(menuName = "Create Machine Gun")]
    public class MachineGunAbility : Ability
    {
        [SerializeField]
        public float MaxDistance = 20;
        
        [SerializeField]
        public float Damage = 4;
    }
}