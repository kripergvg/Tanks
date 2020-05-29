using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Tanks.FSM;
using Tanks.Mobs;
using Tanks.Mobs.Brain.FSMBrain;
using Tanks.Pool;
using Tanks.Tank;
using Tanks.Tank.Abilities;
using UnityEngine;
using UnityEngine.UI;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Random = System.Random;

namespace Tanks.DI
{
    public class StartUpNode : SceneNode
    {
        public TankViewModel TankPrefab;
        public Zombie[] Zombies;
        public Missile MisslePrefab;
        
        public int AliveZombieCount = 10;

        public float ChangeAbilityCooldown = 0.1f;
        
        private StartUpDeps _deps;

        public override void Init(List<SceneNode> sceneNodes)
        {
            var logger = CreateLogger();
            
            _deps = GetDeps(sceneNodes);
            var random = new DefaultRandom(new Random());

            var tankMobTargetLocator = new TypeTargetLocator(EntityType.Tank);
            var targetLocators = new List<ITargetLocator>
            {
                tankMobTargetLocator
            };

            var entitiesSpawner = new EntitiesSpawner(targetLocators);
            var timeProvider = new UnityTimeProvider();
            
            CreateTank(logger, timeProvider, entitiesSpawner);

            var stateMachineFactory = new StateMachineFactory();
            var mobSpawner = CreateMobSpawner(logger, timeProvider, entitiesSpawner, tankMobTargetLocator, stateMachineFactory, random);
            mobSpawner.Start();
        }

        private ILogger CreateLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder => { });
            loggerFactory.AddProvider(new UnityLoggerProvider());
            return loggerFactory.CreateLogger("Client");
        }

        private void CreateTank(ILogger logger, UnityTimeProvider timeProvider, EntitiesSpawner entitiesSpawner)
        {
            var missilePool = new SimplePool<Missile>(logger,
                5,
                2,
                p =>
                {
                    var missile = Instantiate(MisslePrefab);
                    missile.SetPoolOwner(p);
                    return missile;
                });
            var runtimeAbilityFactory = new ScriptableObjectRuntimeAbilityFactory(missilePool);

            var tankFactory = new TankFactory(timeProvider, entitiesSpawner, TankPrefab, _deps.AbilitiesContainer, _deps.PlayerHealth, runtimeAbilityFactory, ChangeAbilityCooldown, new InputManager());
            entitiesSpawner.Spawn<TankFactory, TankFactory, TankViewModel>(tankFactory, tankFactory, _deps.PlayerSpawnPoint.position, _deps.PlayerSpawnPoint.rotation);
        }

        private MobSpawner<ZombieFactory> CreateMobSpawner(ILogger logger,
            UnityTimeProvider timeProvider,
            EntitiesSpawner entitiesSpawner,
            TypeTargetLocator tankMobTargetLocator,
            StateMachineFactory stateMachineFactory,
            DefaultRandom random)
        {
            var zombieFactories = new List<ZombieFactory>();
            foreach (var zombiePrefab in Zombies)
            {
                var pool = new SimplePool<Zombie>(logger,
                    15,
                    10,
                    p =>
                    {
                        var zombie = Instantiate(zombiePrefab);
                        return zombie;
                    });
                var zombieFactory = new ZombieFactory(timeProvider, entitiesSpawner, tankMobTargetLocator, stateMachineFactory, _deps.Doors, pool);
                zombieFactories.Add(zombieFactory);
            }

            var zombieSpawnPoints = GetZombieSpawnPoints(_deps.ZombieSpawnPoints);
            var mobSpawner = new MobSpawner<ZombieFactory>(random, entitiesSpawner, new MobSpawnerSettings(AliveZombieCount, zombieSpawnPoints), zombieFactories);
            return mobSpawner;
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
            Slider playerHealth = null;
            Transform playerSpawnPoint = null;
            Transform[] zombieSpawnPoints = null;
            IDoor[] doors = null;
            
            foreach (var sceneNode in sceneNodes)
            { 
                if (sceneNode is PlayerUiNode playerUiNode)
                {
                    abilitiesContainer = playerUiNode.AbilitiesContainer;
                    playerHealth = playerUiNode.HealthBar;
                }

                if (sceneNode is CoreEnvironmentNode environmentNode)
                {
                    playerSpawnPoint = environmentNode.TankSpawnPoint;
                    zombieSpawnPoints = environmentNode.ZombieSpawnPoints;
                    doors = environmentNode.Doors;
                }
            }

            if (abilitiesContainer == null)
            {
                throw new Exception("abilitiesContainer not found");
            }

            return new StartUpDeps(abilitiesContainer, zombieSpawnPoints, playerSpawnPoint, playerHealth, doors);
        }

        public readonly struct StartUpDeps
        {
            public StartUpDeps(Transform abilitiesContainer,
                Transform[] zombieSpawnPoints, 
                Transform playerSpawnPoint,
                Slider playerHealth,
                IDoor[] doors)
            {
                AbilitiesContainer = abilitiesContainer;
                ZombieSpawnPoints = zombieSpawnPoints;
                PlayerSpawnPoint = playerSpawnPoint;
                PlayerHealth = playerHealth;
                Doors = doors;
            }

            public Transform AbilitiesContainer { get; }
            public Transform[] ZombieSpawnPoints { get; }
            public Transform PlayerSpawnPoint { get; }
            public Slider PlayerHealth { get; }
            public IDoor[] Doors { get; }
        }
    }
}