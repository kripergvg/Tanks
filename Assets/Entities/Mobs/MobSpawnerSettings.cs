using UnityEngine;

namespace Tanks.Mobs
{
    public class MobSpawnerSettings
    {
        public MobSpawnerSettings(int aliveCount, Vector3[] spawnPoints)
        {
            AliveCount = aliveCount;
            SpawnPoints = spawnPoints;
        }

        public int AliveCount { get; }
        public Vector3[] SpawnPoints { get; }
    }
}