using UnityEngine;

namespace Tanks.Tank.Abilities.Ui
{
    public interface IAbilityUi
    {
        void Init(Sprite icon, string name, float cooldown, bool showCooldown);

        void SetFireDate(float date);

        void SetSelected();

        void RemoveSelected();
    }
}