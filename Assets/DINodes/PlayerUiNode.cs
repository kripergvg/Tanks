using UnityEngine;
using UnityEngine.UI;

namespace Tanks.DINodes
{
    public class PlayerUiNode : SceneNode
    {
        [SerializeField]
        public Transform AbilitiesContainer;
        [SerializeField]
        public Slider HealthBar;
    }
}