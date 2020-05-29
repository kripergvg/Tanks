using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tank.Abilities.Ui
{
    public class AbilityUi : MonoBehaviour, IAbilityUi
    {
        [SerializeField]
        private Image _iconElement;
        [SerializeField]
        private TMP_Text _nameElement;
        [SerializeField]
        private Image _notSelectedHover;
        
        private float? _lastFireDate;
        private float _cooldown;
        private bool _showCooldown;

        public void Init(Sprite icon, string name, float cooldown, bool showCooldown)
        {
            _iconElement.sprite = icon;
            _nameElement.text = name;
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
                    _iconElement.fillAmount = Math.Min(1, timeAfterLastShoot / _cooldown);
                }
            }
        }

        public void SetFireDate(float date)
        {
            _lastFireDate = date;
        }

        public void SetSelected()
        {
            _notSelectedHover.enabled = false;
        }

        public void RemoveSelected()
        {
            _notSelectedHover.enabled = true;
        }
    }
}