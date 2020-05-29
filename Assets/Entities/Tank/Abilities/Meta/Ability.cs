using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public abstract class Ability : ScriptableObject, IAbility
    {
        [SerializeField]
        public string Name;

        [SerializeField]
        public Sprite Icon;
        
        [SerializeField]
        public float Cooldown;
        
        [SerializeField]
        public LayerMask TargetsMask;

        [SerializeField]
        public bool ShowCooldow = true;

        string IAbility.Name => Name;
        Sprite IAbility.Icon => Icon;
        float IAbility.Cooldown => Cooldown;
        bool IAbility.ShowCooldown => ShowCooldow;
    }
}