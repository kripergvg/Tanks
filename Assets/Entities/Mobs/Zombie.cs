using System;
using System.Collections;
using Tanks.Entities;
using Tanks.Environment;
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
        [SerializeField]
        private Canvas _uiCanvas;
        [SerializeField]
        private Slider _healthSlider;
        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private float _brainUpdateInterval = 0.02f;
        [SerializeField]
        private float _attackInterval = 0.5f;
        [SerializeField]
        private int _damage = 20;

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
            set => _agent.Warp(value);
        }

        private void Awake()
        {
            _brainUpdateWait = new WaitForSeconds(_brainUpdateInterval);
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
            var mover = new NavMeshMobMover(_agent);
            _attacker = new SimpleAttacker(_damage);
            _targetChaser = new TargetChaser(mover);
            _teleporter = new Teleporter(this);
            _brain = new FSMZombieBrain(timeProvider,
                targetLocator,
                stateMachineFactory,
                _targetChaser,
                _attacker,
                TimeSpan.FromSeconds(_attackInterval),
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
            _healthSlider.value = _healthSystem.GetHealthInPercent();

            if (_cameraStorage.Exists())
            {
                _uiCanvas.transform.LookAt(_cameraStorage.Get().transform);
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