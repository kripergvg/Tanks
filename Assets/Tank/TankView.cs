using System;
using UnityEngine;

namespace Tanks
{
    public class TankView : MonoBehaviour
    {
        public event Action OnFire;

        public int Speed = 2;
        public int RotationSpeed = 2;
        public Rigidbody RigidBody;

        private int _leftRotatePressedCount;
        private int _rightRotatePressedCount;
        private int _forwardPressedCount;
        private int _backPressedCount;

        private int _lastFixedUpdateFrameCount;
        private int _frameCountDelta;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // TODO Поворачивать нуэно стрелками 
            ReceiveInput(KeyCode.A, ref _leftRotatePressedCount);
            ReceiveInput(KeyCode.D, ref _rightRotatePressedCount);
            ReceiveInput(KeyCode.W, ref _forwardPressedCount);
            // TODO При движении назад повороты не в ту сторону
            ReceiveInput(KeyCode.S, ref _backPressedCount);

            if (Input.GetKey(KeyCode.X))
            {
                OnFire?.Invoke();
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