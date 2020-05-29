using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public abstract class Ability : ScriptableObject, IAbility
    {
        public string Name;

        public Sprite Icon;

        public float Cooldown;
        
        public LayerMask TargetsMask;

        public bool ShowCooldow = true;

        string IAbility.Name => Name;
        Sprite IAbility.Icon => Icon;
        float IAbility.Cooldown => Cooldown;
        bool IAbility.ShowCooldown => ShowCooldow;
    }

    public interface IAbility
    {
        string Name { get; }
        Sprite Icon { get; }
        float Cooldown { get; }
        bool ShowCooldown { get; }
    }
}