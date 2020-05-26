using System;
using Tanks.Mobs;
using Tanks.Tank.Abilities;
using UnityEngine;

namespace Tanks.Tank
{
    public class TankViewModel : Entity
    {
        public Ability[] Abilities;
        public AbilityUi AbilityUiPrefab;
        public float ChangeAbilityCooldown = 0.1f;
        public Camera PlayerCamera;
        public int Speed = 2;
        public int RotationSpeed = 2;
        public Rigidbody RigidBody;
        public Transform TowerEnd;
        
        private int _leftRotatePressedCount;
        private int _rightRotatePressedCount;
        private int _forwardPressedCount;
        private int _backPressedCount;

        private int _lastFixedUpdateFrameCount;
        private int _frameCountDelta;
        private AbilitySystem _abilitySystem;
        private float? _lastChangedAbilityTime;

        public override EntityType EntityType { get; } = EntityType.Tank;
        
        public void Init(AbilitySystem abilitySystem, Transform abilitiesContainer, MainCameraStorage mainCameraStorage)
        {
            mainCameraStorage.Set(PlayerCamera);
            _abilitySystem = abilitySystem;
            abilitySystem.Init(Abilities, new ToParentFactory<AbilityUi, IAbilityUi>(AbilityUiPrefab, abilitiesContainer));
        }

        // Update is called once per frame
        void Update()
        {
            // TODO Поворачивать нуэно стрелками 
            ReceiveInput(KeyCode.LeftArrow, ref _leftRotatePressedCount);
            ReceiveInput(KeyCode.RightArrow, ref _rightRotatePressedCount);
            ReceiveInput(KeyCode.UpArrow, ref _forwardPressedCount);
            // TODO При движении назад повороты не в ту сторону
            ReceiveInput(KeyCode.DownArrow, ref _backPressedCount);

            if (Input.GetKey(KeyCode.X))
            {
                var context = new AbilityContext(transform, TowerEnd);
                _abilitySystem.Fire(ref context);
            }

            if (_lastChangedAbilityTime == null
                || Time.time - _lastChangedAbilityTime > ChangeAbilityCooldown)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    _abilitySystem.PreviousAbility();
                    _lastChangedAbilityTime = Time.time;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    _abilitySystem.NextAbility();
                    _lastChangedAbilityTime = Time.time;
                }
            }
        }

        private void ReceiveInput(KeyCode keyCode, ref int counter)
        {
            if (Input.GetKey(keyCode))
            {
                counter++;
            }
        }

        // TODO Отрефакторить
        private void ClearInput(ref int counter)
        {
            counter = 0;
        }

        private void FixedUpdate()
        {
            _frameCountDelta = Time.frameCount - _lastFixedUpdateFrameCount;
            if (_frameCountDelta == 0)
            {
                return;
            }

            var verticalValue = GetInputValue(_forwardPressedCount, _backPressedCount);
            var movement = transform.forward * (verticalValue * Speed);
            RigidBody.velocity = new Vector3(movement.x, RigidBody.velocity.y, movement.z);

            var horizontalValue = GetInputValue(_rightRotatePressedCount, _leftRotatePressedCount);
            RigidBody.angularVelocity = Vector3.up * (horizontalValue * RotationSpeed);

            _lastFixedUpdateFrameCount = Time.frameCount;

            ClearInput(ref _leftRotatePressedCount);
            ClearInput(ref _rightRotatePressedCount);
            ClearInput(ref _forwardPressedCount);
            ClearInput(ref _backPressedCount);
        }

        private int GetInputValue(int positivePressedCount, int negativePressedCount)
        {
            var diff = positivePressedCount - negativePressedCount;
            return diff / _frameCountDelta;
        }
    }
}