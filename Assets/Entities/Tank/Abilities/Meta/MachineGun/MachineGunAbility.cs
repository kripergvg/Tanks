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
        
        public override void Fire(in AbilityContext context)
        {
            var hitBuffer= ArrayPool<RaycastHit>.Shared.Rent(100);

            var hitCount= Physics.RaycastNonAlloc(context.RootTransform.position, context.RootTransform.forward, hitBuffer, MaxDistance, TargetsMask);
            for (int i = 0; i < hitCount; i++)
            {
                var entity = hitBuffer[i].transform.GetComponent<IEntity>();
                entity.TakeDamage(Damage);
            }
                
            ArrayPool<RaycastHit>.Shared.Return(hitBuffer);
        }
    }
}