using UnityEngine;

namespace Entities.Tank.Abilities.Meta.MachineGun
{
    public interface IMachineGunVisualizer
    {
        void Visualize();
    }

    public class MachineGunVisualizer : IMachineGunVisualizer
    {
        private readonly LineRenderer _shootRender;
        private readonly float _visualizationDuration;
        private float? _lastRenderTime;
        private bool _visualizing;

        public MachineGunVisualizer(LineRenderer shootRender, float visualizationDuration)
        {
            _shootRender = shootRender;
            _visualizationDuration = visualizationDuration;
        }


        public void Update()
        {
            if (_visualizing && Time.time - _lastRenderTime > _visualizationDuration)
            {
                _visualizing = false;
                _shootRender.enabled = false;
            }
        }

        public void Visualize()
        {
            _lastRenderTime = Time.time;
            _shootRender.enabled = true;
            _visualizing = true;
        }
    }
}