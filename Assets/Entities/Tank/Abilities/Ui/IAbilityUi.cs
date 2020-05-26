using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public interface IAbilityUi
    {
        void Init(Sprite icon, string name, float cooldown);

        void SetFireDate(float date);

        void SetSelected();

        void RemoveSelected();
    }
}