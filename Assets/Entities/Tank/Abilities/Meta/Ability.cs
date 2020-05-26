using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public abstract class Ability : ScriptableObject, IAbility
    {
        public string Name;

        public Sprite Icon;

        public float Cooldown;

        string IAbility.Name => Name;
        Sprite IAbility.Icon => Icon;
        float IAbility.Cooldown => Cooldown;
        
        public abstract void Fire(ref AbilityContext context);
    }

    public interface IAbility
    {
        string Name { get; }
        Sprite Icon { get; }
        float Cooldown { get; }
        void Fire(ref AbilityContext context);
    }
}