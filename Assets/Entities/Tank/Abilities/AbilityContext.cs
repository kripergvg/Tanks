using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public readonly struct AbilityContext
    {
        public AbilityContext(Transform rootTransform, Transform towerEnd)
        {
            RootTransform = rootTransform;
            TowerEnd = towerEnd;
        }
        
        public Transform RootTransform { get; }
        public Transform TowerEnd { get; }
    }
}