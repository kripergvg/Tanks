using System;
using System.Collections;
using Tanks.DI;
using Tanks.FSM;
using Tanks.Mobs.Brain;
using Tanks.Mobs.Brain.FSMBrain;
using Tanks.Pool;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Tanks.Mobs
{
    public class Zombie : Entity, IPoolable
    {
        public Canvas UiCanvas;
        public Slider HealthSlider;
        public NavMeshAgent Agent;

        public float BrainUpdateInterval = 0.02f;
        public float AttackInterval = 0.5f;
        public int Damage = 20;

        private MainCameraStorage _cameraStorage;
        private HealthSystem _healthSystem;
        private WaitForSeconds _brainUpdateWait;
        private FSMZombieBrain _brain;
        private TargetChaser _targetChaser;
        private SimpleAttacker _attacker;
        private Teleporter _teleporter;
        
        public override EntityType EntityType { get; } = EntityType.Zombie;

        public override Vector3 Position
        {
            get => transform.position;
            set => Agent.Warp(value);
        }

        private void Awake()
        {
            _brainUpdateWait = new WaitForSeconds(BrainUpdateInterval);
        }

        public void Init(ITimeProvider timeProvider,
            HealthSystem healthSystem, 
            MainCameraStorage mainCameraStorage, 
            ITargetLocator targetLocator,
            StateMachineFactory stateMachineFactory,
            IDoor[] doors)
        {
            _healthSystem = healthSystem;
            _cameraStorage = mainCameraStorage;
            var mover = new NavMeshMobMover(Agent);
            _attacker = new SimpleAttacker(Damage);
            _targetChaser = new TargetChaser(mover);
            _teleporter = new Teleporter(this);
            _brain = new FSMZombieBrain(timeProvider,
                targetLocator,
                stateMachineFactory,
                this,
                _targetChaser,
                _attacker,
                TimeSpan.FromSeconds(AttackInterval),
                doors,
                mover,
                _teleporter);
            base.Init(_healthSystem);
        }
        
        public void PoolInit()
        {
            gameObject.SetActive(true);
        }

        public override void OnSpawn()
        {
            _brain.Start();
            StartCoroutine(BrainUpdate(_brain));
            base.OnSpawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            _teleporter.OnTriggerEnter(other);

            if (other.TryGetComponent<IEntity>(out var entity))
            {
                _targetChaser.OnEntityTriggerEnter(entity);
                _brain.OnEntityTriggerEnter(entity);
                _attacker.OnEntityTriggerEnter(entity);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<IEntity>(out var entity))
            {
                _targetChaser.OnEntityTriggerStay(entity);
                _attacker.OnEntityTriggerStay(entity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IEntity>(out var entity))
            {
                _targetChaser.OnEntityTriggerExit(entity);
                _brain.OnEntityTriggerExit(entity);
            }
        }

        IEnumerator BrainUpdate(IZombieBrain zombieBrain)
        {
            zombieBrain.Start();
            while (true)
            {
                zombieBrain.Update();
            
                yield return _brainUpdateWait;
            }
            // ReSharper disable once IteratorNeverReturns - stops on destroy
        }

        void LateUpdate()
        {
            HealthSlider.value = _healthSystem.GetHealthInPercent();

            if (_cameraStorage.Exists())
            {
                UiCanvas.transform.LookAt(_cameraStorage.Get().transform);
            }
        }

        public override void BeforeDespawnEntity()
        {
            _brain.Stop();
        }

        public void PoolClear()
        {
            gameObject.SetActive(false);
        }
    }
}