using Tanks.Environment;
using UnityEngine;

namespace Tanks.DINodes
{
    public class CoreEnvironmentNode : SceneNode
    {
        [SerializeField]
        public Transform TankSpawnPoint;
        [SerializeField]
        public Transform[] ZombieSpawnPoints;
        [SerializeField]
        public Door[] Doors;
    }
}