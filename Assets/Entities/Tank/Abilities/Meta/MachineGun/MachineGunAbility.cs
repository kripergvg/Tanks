using System.Buffers;
using Tanks.Mobs;
using UnityEngine;

namespace Tanks.Tank.Abilities.MachineGun
{
    [CreateAssetMenu(menuName = "Create Machine Gun")]
    public class MachineGunAbility : Ability
    {
        public float MaxDistance = 20;
        public float Damage = 4;
    }
}