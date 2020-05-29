using Entities.Tank.Abilities.Meta.MachineGun;
using Tanks.Mobs;
using Tanks.Tank.Abilities;
using Tanks.Tank.Abilities.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tank
{
    public class TankViewModel : Entity
    {
        [SerializeField]
        private Ability[] _abilities;
        [SerializeField]
        private AbilityUi _abilityUiPrefab;
        [SerializeField]
        private Camera _playerCamera;
        [SerializeField]
        private int _speed = 2;
        [SerializeField]
        private float _rotationSpeed = 1.6f;
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private Transform _towerEnd;
        [SerializeField]
        private LineRenderer _machineGunLineRenderer;
        [SerializeField]
        private float _machineGunLineDuration = 0.1f;
        
        private AbilitySystem _abilitySystem;
        private HealthSystem _healthSystem;
        private AbilityContext _abilityContext;
        private Slider _playerHealth;
        private InputManager _inputManager;
        private MachineGunVisualizer _machineGunVisualizer;

        public override EntityType EntityType { get; } = EntityType.Tank;

        public void Init(HealthSystem healthSystem,
            AbilitySystem abilitySystem,
            Transform abilitiesContainer,
            MainCameraStorage mainCameraStorage,
            Slider playerHealth,
            InputManager inputManager)
        {
            _healthSystem = healthSystem;
            _playerHealth = playerHealth;
            _abilitySystem = abilitySystem;
            _machineGunVisualizer = new MachineGunVisualizer(_machineGunLineRenderer, _machineGunLineDuration);
            var deps = new TankDeps(_machineGunVisualizer);
            abilitySystem.Init(_abilities, new ToParentFactory<AbilityUi, IAbilityUi>(_abilityUiPrefab, abilitiesContainer), in deps);
            mainCameraStorage.Set(_playerCamera);

            _inputManager = inputManager;
            inputManager.OnAbilityPressed += () => { _abilitySystem.Fire(in _abilityContext); };
            inputManager.OnNextAbilityPressed += _abilitySystem.NextAbility;
            inputManager.OnPreviousAbilityPressed += _abilitySystem.PreviousAbility;
            
            _abilityContext = new AbilityContext(transform, _towerEnd);
            base.Init(healthSystem);
        }

        // Update is called once per frame
        void Update()
        {
            _machineGunVisualizer.Update();
            _inputManager.ReadInput();
        }
        
        private void FixedUpdate()
        {
            var movementData = _inputManager.GetMovementData();
            _inputManager.ClearMovementData();
            var verticalValue = GetInputValue(movementData.Forward, movementData.Backwards);
            var movement = transform.forward * (verticalValue * _speed);
            _rigidBody.velocity = new Vector3(movement.x, _rigidBody.velocity.y, movement.z);

            var horizontalValue = GetInputValue(movementData.Right, movementData.Left);
            // backwards moving
            if (verticalValue < 0)
            {
                horizontalValue *= -1;
            }
         
            _rigidBody.angularVelocity = Vector3.up * (horizontalValue * _rotationSpeed);
        }

        private int GetInputValue(bool positivePressed, bool negativePressed)
        {
            if (positivePressed && negativePressed)
            {
                return 0;
            }

            if (positivePressed)
            {
                return 1;
            }

            if (negativePressed)
            {
                return -1;
            }

            return 0;
        }

        private void LateUpdate()
        {
            _playerHealth.value = _healthSystem.GetHealthInPercent();
        }
    }
}