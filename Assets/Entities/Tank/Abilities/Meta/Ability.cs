using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public abstract class Ability : ScriptableObject, IAbility
    {
        public string Name;

        public Sprite Icon;

        public float Cooldown;
        
        public LayerMask TargetsMask;

        string IAbility.Name => Name;
        Sprite IAbility.Icon => Icon;
        float IAbility.Cooldown => Cooldown;
        
        public abstract void Fire(in AbilityContext context);
    }

    public interface IAbility
    {
        string Name { get; }
        Sprite Icon { get; }
        float Cooldown { get; }
        void Fire(in AbilityContext context);
    }
}