namespace Tanks.Tank.Abilities
{
    public interface IRuntimeAbilityFactory
    {
        IRuntimeAbility Create(IAbility abilitySettings, in TankDeps tankDeps);
    }
}