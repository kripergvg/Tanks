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
        public Image SelectedHover;
        
        private float? _lastFireDate;
        private float _cooldown;

        public void Init(Sprite icon, string name, float cooldown)
        {
            IconElement.sprite = icon;
            NameElement.text = name;
            _cooldown = cooldown;
        }

        private void Update()
        {
            if (_lastFireDate != null)
            {
                var timeAfterLastShoot = Time.time - _lastFireDate.Value;
                IconElement.fillAmount = Math.Min(1, timeAfterLastShoot / _cooldown);
            }
        }

        public void SetFireDate(float date)
        {
            _lastFireDate = date;
        }

        public void SetSelected()
        {
            SelectedHover.enabled = true;
        }

        public void RemoveSelected()
        {
            SelectedHover.enabled = false;
        }
            
    }
}