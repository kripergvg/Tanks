using System;
using System.Collections.Generic;
using Tanks.Mobs;
using Tanks.Tank;
using UnityEngine;
using Random = System.Random;

namespace Tanks.DI
{
    public class StartUpNode : SceneNode
    {
        public TankViewModel TankPrefab;
        public Zombie[] Zombies;
        
        private StartUpDeps _deps;

        public override void Init(List<SceneNode> sceneNodes)
        {
            _deps = GetDeps(sceneNodes);
            var random = new DefaultRandom(new Random());

            var entitiesDestroyer = new UnityEntityDestroyer();
            var entitiesSpawner = new EntitiesSpawner(entitiesDestroyer);
            var timeProvider = new UnityTimeProvider();

            var tankFactory = new TankFactory(timeProvider, TankPrefab, _deps.AbilitiesContainer);
            entitiesSpawner.Spawn<TankFactory, TankViewModel>(tankFactory, _deps.PlayerSpawnPoint.position, _deps.PlayerSpawnPoint.rotation);

            var zombieFactories = new List<ZombieFactory>();
            foreach (var zombyPrefab in Zombies)
            {
                var zombieFactory = new ZombieFactory(zombyPrefab);
                zombieFactories.Add(zombieFactory);
            }

            var zombieSpawnPoints = GetZombieSpawnPoints(_deps.ZombieSpawnPoints);
            var mobSpawner = new MobSpawner<ZombieFactory>(random, entitiesSpawner, new MobSpawnerSettings(10, zombieSpawnPoints), zombieFactories);
            mobSpawner.Start();
        }

        private Vector3[] GetZombieSpawnPoints(Transform[] zombieSpawnPoints)
        {
            var zombieSpawnPointsPositions = new Vector3[zombieSpawnPoints.Length];
            for (var index = 0; index < zombieSpawnPoints.Length; index++)
            {
                zombieSpawnPointsPositions[index] = zombieSpawnPoints[index].position;
            }

            return zombieSpawnPointsPositions;
        }

        private StartUpDeps GetDeps(List<SceneNode> sceneNodes)
        {
            Transform abilitiesContainer = null;
            Transform playerSpawnPoint = null;
            Transform[] zombieSpawnPoints = null;
            foreach (var sceneNode in sceneNodes)
            { 
                if (sceneNode is PlayerUiNode playerUiNode)
                {
                    abilitiesContainer = playerUiNode.AbilitiesContainer;
                }

                if (sceneNode is CoreEnvironmentNode environmentNode)
                {
                    playerSpawnPoint = environmentNode.TankSpawnPoint;
                    zombieSpawnPoints = environmentNode.ZombieSpawnPoints;
                }
            }

            if (abilitiesContainer == null)
            {
                throw new Exception("abilitiesContainer not found");
            }

            return new StartUpDeps(abilitiesContainer, zombieSpawnPoints, playerSpawnPoint);
        }

        public readonly struct StartUpDeps
        {
            public StartUpDeps(Transform abilitiesContainer, Transform[] zombieSpawnPoints, Transform playerSpawnPoint)
            {
                AbilitiesContainer = abilitiesContainer;
                ZombieSpawnPoints = zombieSpawnPoints;
                PlayerSpawnPoint = playerSpawnPoint;
            }

            public Transform AbilitiesContainer { get; }
            public Transform[] ZombieSpawnPoints { get; }
            public Transform PlayerSpawnPoint { get; }
        }
    }
}