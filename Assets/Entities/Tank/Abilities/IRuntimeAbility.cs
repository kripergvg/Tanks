namespace Tanks.Tank.Abilities
{
    public interface IRuntimeAbility
    {
        void Fire(in AbilityContext context);
    }
}