using Entities.Tank.Abilities.Meta.MachineGun;

namespace Tanks.Tank.Abilities
{
    public readonly struct TankDeps
    {
        public TankDeps(IMachineGunVisualizer machineGunVisualizer)
        {
            MachineGunVisualizer = machineGunVisualizer;
        }

        public IMachineGunVisualizer MachineGunVisualizer { get; }
    }
}