using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public interface IAbility
    {
        string Name { get; }
        Sprite Icon { get; }
        float Cooldown { get; }
        bool ShowCooldown { get; }
    }
}