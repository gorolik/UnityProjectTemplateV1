using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Sources.Services.Input
{
    public class KeyboardMouseInputService : IInputService
    {
        private readonly GameInputActions _inputActions;

        public GameInputActions InputActions => _inputActions;

        public Action<DeviceType> OnCurrentDeviceChanged { get => _onCurrentDeviceChanged; set => _onCurrentDeviceChanged = value; }

        private bool _isGamepadUsed;
        private Action<DeviceType> _onCurrentDeviceChanged;

        public KeyboardMouseInputService()
        {
            _inputActions = new GameInputActions();
            _inputActions.Enable();
            
            InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
        }

        private void OnAnyButtonPress(InputControl inputControl)
        {
            if (inputControl.device is Gamepad)
            {
                OnCurrentDeviceChanged?.Invoke(DeviceType.Console);
                _isGamepadUsed = true;
            }
            else
            {
                OnCurrentDeviceChanged?.Invoke(DeviceType.Desktop);
                _isGamepadUsed = false;
            }
        }

        public virtual Vector2 GetLookDirection() => 
            _inputActions.Player.Look.ReadValue<Vector2>();

        public virtual Vector2 GetMoveDirection() => 
            _inputActions.Player.Move.ReadValue<Vector2>();

        public bool IsFireDown() => 
            _inputActions.Player.Attack.WasPressedThisFrame();

        public bool IsFireUp() => 
            _inputActions.Player.Attack.WasReleasedThisFrame();

        public bool IsJumpDown() => 
            _inputActions.Player.Jump.WasPressedThisFrame();

        public bool IsCrouchDown() => 
            _inputActions.Player.Crouch.WasPressedThisFrame();

        public bool IsCrouchUp() => 
            _inputActions.Player.Crouch.WasReleasedThisFrame();

        public bool IsRunning() => 
            _inputActions.Player.Sprint.IsPressed();
        
        public bool IsCursorLockButtonPerformed() => 
            _inputActions.Player.SwitchCursorState.WasPerformedThisFrame();

        public virtual bool IsMobilePlatform() => 
            false;
        
        public virtual bool IsJoystickInUse() => 
            _isGamepadUsed;
    }
}
