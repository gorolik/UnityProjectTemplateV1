using System;
using UnityEngine;

namespace Sources.Services.Input
{
    public interface IInputService
    {
        Action<DeviceType> OnCurrentDeviceChanged { get; set; }
        GameInputActions InputActions { get; }
        
        bool IsMobilePlatform();
        bool IsJoystickInUse();
        
        Vector2 GetLookDirection();
        Vector2 GetMoveDirection();
        bool IsFireDown();
        bool IsFireUp();
        bool IsJumpDown();
        bool IsCrouchDown();
        bool IsCrouchUp();
        bool IsRunning();
        bool IsCursorLockButtonPerformed();
    }
}
