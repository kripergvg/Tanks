using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tank.Abilities
{
    public class AbilityUi : MonoBehaviour, IAbilityUi
    {
        public Image IconElement;
        public TMP_Text NameElement;
        public Image NotSelectedHover;
        
        private float? _lastFireDate;
        private float _cooldown;
        private bool _showCooldown;

        public void Init(Sprite icon, string name, float cooldown, bool showCooldown)
        {
            IconElement.sprite = icon;
            NameElement.text = name;
            _cooldown = cooldown;
            _showCooldown = showCooldown;
        }

        private void Update()
        {
            if (_showCooldown)
            {
                if (_lastFireDate != null)
                {
                    var timeAfterLastShoot = Time.time - _lastFireDate.Value;
                    IconElement.fillAmount = Math.Min(1, timeAfterLastShoot / _cooldown);
                }
            }
        }

        public void SetFireDate(float date)
        {
            _lastFireDate = date;
        }

        public void SetSelected()
        {
            NotSelectedHover.enabled = false;
        }

        public void RemoveSelected()
        {
            NotSelectedHover.enabled = true;
        }
    }
}