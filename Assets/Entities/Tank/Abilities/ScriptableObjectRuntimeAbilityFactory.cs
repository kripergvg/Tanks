using System;
using Tanks.Pool;
using Tanks.Tank.Abilities.MachineGun;
using Tanks.Tank.Abilities.Missile;

namespace Tanks.Tank.Abilities
{
    public class ScriptableObjectRuntimeAbilityFactory : IRuntimeAbilityFactory
    {
        private readonly IPool<Missile.Missile> _misslePool;

        public ScriptableObjectRuntimeAbilityFactory(IPool<Missile.Missile> misslePool)
        {
            _misslePool = misslePool;
        }
        
        public IRuntimeAbility Create(IAbility abilitySettings, in TankDeps tankDeps)
        {
            switch (abilitySettings)
            {
                case MissileAbility missileAbility:
                    return new MissileRuntimeAbility(_misslePool, missileAbility);
                case MachineGunAbility machineGunAbility:
                    return new MachineGunRuntimeAbility(machineGunAbility, tankDeps.MachineGunVisualizer);
                default:
                    throw new ArgumentOutOfRangeException("Ability is not supported");
            }
        }
    }
}