using System;
using Entities.Tank.Abilities.Meta.MachineGun;
using Tanks.Mobs;
using Tanks.Mobs.Brain.FSMBrain;
using Tanks.Tank.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tank
{
    public class TankViewModel : Entity
    {
        public Ability[] Abilities;
        public AbilityUi AbilityUiPrefab;
        public Camera PlayerCamera;
        public int Speed = 2;
        public float RotationSpeed = 1.6f;
        
        public Rigidbody RigidBody;
        public Transform TowerEnd;
        public LineRenderer MachineGunLineRenderer;
        public float MachineGunLineDuration = 0.1f;
        
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
            _machineGunVisualizer = new MachineGunVisualizer(MachineGunLineRenderer, MachineGunLineDuration);
            var deps = new TankDeps(_machineGunVisualizer);
            abilitySystem.Init(Abilities, new ToParentFactory<AbilityUi, IAbilityUi>(AbilityUiPrefab, abilitiesContainer), in deps);
            mainCameraStorage.Set(PlayerCamera);

            _inputManager = inputManager;
            inputManager.OnAbilityPressed += () => { _abilitySystem.Fire(in _abilityContext); };
            inputManager.OnNextAbilityPressed += _abilitySystem.NextAbility;
            inputManager.OnPreviousAbilityPressed += _abilitySystem.PreviousAbility;
            
            _abilityContext = new AbilityContext(transform, TowerEnd);
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
            var movement = transform.forward * (verticalValue * Speed);
            RigidBody.velocity = new Vector3(movement.x, RigidBody.velocity.y, movement.z);

            var horizontalValue = GetInputValue(movementData.Right, movementData.Left);
            // backwards moving
            if (verticalValue < 0)
            {
                horizontalValue *= -1;
            }
         
            RigidBody.angularVelocity = Vector3.up * (horizontalValue * RotationSpeed);
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