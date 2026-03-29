using PlayerController.Scripts.Implementation.Model;
using Sources.Services.Input;
using UnityEngine;
using Zenject;

namespace PlayerController.Scripts.Implementation.Entities
{
    public class BasePlayerMovement : KinematicEntity
    {
        [Header("Camera")]
        [SerializeField] private BaseCharacterCamera _characterCamera;
        [SerializeField] private Transform _cameraPoint;
        [Header("Crouching")] 
        [SerializeField] private float _standCameraHeight = 1.5f;
        [Tooltip("Only for ColliderStayBottom crouching type")]
        [SerializeField] private float _crouchingCameraHeight = 0.5f;

        private IInputService _inputService;
        
        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;

        private void Start()
        {
            _characterCamera.SetFollowTransform(_cameraPoint);

            Character.OnCrouch += OnCrouch;
            Character.OnStand += OnStand;
        }

        private void OnDestroy()
        {
            Character.OnCrouch -= OnCrouch;
            Character.OnStand -= OnStand;
        }

        private void LateUpdate()
        {
            HandleCameraInput();
            HandleCharacterInput();
        }

        public override void Teleport(Vector3 point)
        {
            base.Teleport(point);
            _characterCamera.transform.position = point;
        }
        
        public override void SetViewEuler(Vector3 euler)
        {
            base.SetViewEuler(euler);
            _characterCamera.LookInEuler(euler);
        }

        public override void SetViewToPoint(Vector3 lookPoint)
        {
            base.SetViewToPoint(lookPoint);
            _characterCamera.LookAtPoint(lookPoint, Character.Motor.CharacterUp);
        }

        private void HandleCharacterInput()
        {
            Vector2 moveDirection = _inputService.GetMoveDirection();
            
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {
                MoveAxisForward = moveDirection.y,
                MoveAxisRight = moveDirection.x,
                CameraRotation = _characterCamera.transform.rotation,
                Running = _inputService.IsRunning(),
                JumpDown = _inputService.IsJumpDown(),
                CrouchDown = _inputService.IsCrouchDown(),
                CrouchUp = _inputService.IsCrouchUp()
            };

            Character.SetInputs(ref characterInputs);
        }

        private void HandleCameraInput()
        {
            Vector2 lookDirection = _inputService.GetLookDirection();
            Vector3 lookInputVector = new Vector3(lookDirection.x, lookDirection.y, 0f);
            
            if (Cursor.lockState != CursorLockMode.Locked)
                lookInputVector = Vector3.zero;
            
            _characterCamera.UpdateWithInput(lookInputVector, Time.deltaTime);
        }
        
        private void OnCrouch()
        {
            if (Character.ChrouchType == CrouchType.ColliderStayBottom)
                SetCameraPointHeight(_crouchingCameraHeight);
        }

        private void OnStand() => 
            SetCameraPointHeight(_standCameraHeight);

        private void SetCameraPointHeight(float height)
        {
            _cameraPoint.localPosition = new Vector3(_cameraPoint.localPosition.x, height,
                _cameraPoint.localPosition.z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_cameraPoint == null)
                return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(
                _cameraPoint.position.x, Character.transform.position.y + _standCameraHeight, _cameraPoint.position.z),
                0.1f);

            if (Character.ChrouchType == CrouchType.ColliderStayBottom)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(new Vector3(
                        _cameraPoint.position.x, Character.transform.position.y + _crouchingCameraHeight,
                        _cameraPoint.position.z),
                    0.1f);
            }
        }
        
        private void OnValidate() => 
            SyncPointAndCameraPosition();
        
        /// <summary>
        /// Задаёт высоту _cameraPoint в зависимости от _standCameraHeight
        /// и синхронизирует положение _characterCamera с _cameraPoint
        /// </summary>
        [ContextMenu("Sync Point And Camera Position")]
        public void SyncPointAndCameraPosition()
        {

            if (_cameraPoint != null)
            {
                SetCameraPointHeight(_standCameraHeight);

                if (_characterCamera != null)
                {
                    _characterCamera.transform.position = _cameraPoint.position;
                    _characterCamera.transform.rotation = _cameraPoint.rotation;
                }
                else
                {
                    Debug.LogWarning("No assigned character camera");
                }
            }
            else
            {
                Debug.LogWarning("No assigned camera point");
            }
        }
#endif
    }
}
