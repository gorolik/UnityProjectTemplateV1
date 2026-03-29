using UnityEngine;

namespace PlayerController.Scripts.Implementation.Model
{
    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool Running;
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;
    }
}