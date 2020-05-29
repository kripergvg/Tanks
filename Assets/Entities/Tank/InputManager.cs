using System;
using UnityEngine;

namespace Tanks.Tank
{
    public class InputManager
    {
        private bool _leftPressed;
        private bool _rightPressed;
        private bool _forwardPressed;
        private bool _backwardPressed;
        
        public event Action OnAbilityPressed;
        public event Action OnPreviousAbilityPressed;
        public event Action OnNextAbilityPressed;

        public void ReadInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _leftPressed = true;
            }
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _rightPressed = true;
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _forwardPressed = true;
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _backwardPressed = true;
            }

            if (Input.GetKey(KeyCode.X))
            {
                OnAbilityPressed?.Invoke();
            }

            if (Input.GetKey(KeyCode.Q))
            {
                OnPreviousAbilityPressed?.Invoke();
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                OnNextAbilityPressed?.Invoke();
            }
        }

        public MovementData GetMovementData()
        {
            return new MovementData(_leftPressed, _rightPressed, _backwardPressed, _forwardPressed);
        }

        public void ClearMovementData()
        {
            _leftPressed = false;
            _rightPressed = false;
            _forwardPressed = false;
            _backwardPressed = false;
        }

        public readonly struct MovementData
        {
            public MovementData(bool left, bool right, bool backwards, bool forward)
            {
                Left = left;
                Right = right;
                Backwards = backwards;
                Forward = forward;
            }

            public bool Left { get; }
            public bool Right { get; }
            public bool Backwards { get; }
            public bool Forward { get; }
        }
    }
}