using System.Buffers;
using Entities.Tank.Abilities.Meta.MachineGun;
using Tanks.Mobs;
using Tanks.Tank.Abilities.MachineGun;
using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class MachineGunRuntimeAbility : IRuntimeAbility
    {
        private readonly MachineGunAbility _machineGunAbilityMetaInfo;
        private readonly IMachineGunVisualizer _visualizer;

        public MachineGunRuntimeAbility(MachineGunAbility machineGunAbilityMetaInfo, IMachineGunVisualizer visualizer)
        {
            _machineGunAbilityMetaInfo = machineGunAbilityMetaInfo;
            _visualizer = visualizer;
        }
        
        public void Fire(in AbilityContext context)
        {
            var hitBuffer = ArrayPool<RaycastHit>.Shared.Rent(100);
            try
            {
                var hitCount = Physics.RaycastNonAlloc(context.RootTransform.position,
                    context.RootTransform.forward,
                    hitBuffer,
                    _machineGunAbilityMetaInfo.MaxDistance,
                    _machineGunAbilityMetaInfo.TargetsMask);
                for (int i = 0; i < hitCount; i++)
                {
                    var entity = hitBuffer[i].transform.GetComponent<IEntity>();
                    entity.TakeDamage(_machineGunAbilityMetaInfo.Damage);
                }
                
                _visualizer.Visualize();
            }
            finally
            {
                ArrayPool<RaycastHit>.Shared.Return(hitBuffer);
            }
        }
    }
}