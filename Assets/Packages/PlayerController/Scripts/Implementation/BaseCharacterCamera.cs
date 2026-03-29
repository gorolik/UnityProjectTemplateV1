using UnityEngine;

namespace PlayerController.Scripts.Implementation
{
    public class BaseCharacterCamera : MonoBehaviour
    {
        [Header("Farming")]
        public float FollowingSharpness = 10000f;
        [Header("Rotation")] 
        public bool InvertX;
        public bool InvertY;
        [Range(-90f, 90f)] public float DefaultVerticalAngle = 20f;
        [Range(-90f, 90f)] public float MinVerticalAngle = -90f;
        [Range(-90f, 90f)] public float MaxVerticalAngle = 90f;
        public float RotationSpeed = 1f;
        public float RotationSharpness = 10000f;

        public Transform FollowTransform { get; private set; }
        public Vector3 PlanarDirection { get; set; }

        private float _targetVerticalAngle;
        private Vector3 _currentFollowPosition;
        
        private void Awake()
        {
            _targetVerticalAngle = 0f;
            PlanarDirection = Vector3.forward;
        }

        private void OnValidate() => 
            DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);

        public void SetFollowTransform(Transform t)
        {
            FollowTransform = t;
            PlanarDirection = FollowTransform.forward;
            _currentFollowPosition = FollowTransform.position;
        }

        public void UpdateWithInput(Vector3 rotationInput, float deltaTime)
        {
            CameraRotation(rotationInput, deltaTime);
            CameraMovement(deltaTime);
        }
        
        public void LookInDirection(Vector3 worldDirection, Vector3 up)
        {
            if (worldDirection.sqrMagnitude < 0.0001f)
                return;

            Vector3 planarDirection = Vector3.ProjectOnPlane(worldDirection, up).normalized;
            float verticalAngle = Vector3.SignedAngle(planarDirection, worldDirection.normalized, Vector3.Cross(planarDirection, up));

            PlanarDirection = planarDirection;
            SetVerticalLookAngle(verticalAngle);
        }

        public void LookAtPoint(Vector3 targetPoint, Vector3 up)
        {
            Vector3 toTarget = targetPoint - transform.position;
            LookInDirection(toTarget, up);
        }

        public void LookInEuler(Vector3 euler)
        {
            PlanarDirection = Quaternion.Euler(0f, euler.y, 0f) * Vector3.forward;
            SetVerticalLookAngle(euler.x);
        }

        private void SetVerticalLookAngle(float angle) =>
            _targetVerticalAngle = Mathf.Clamp(-angle, MinVerticalAngle, MaxVerticalAngle);

        private void CameraRotation(Vector3 rotationInput, float deltaTime)
        {
            if (InvertX)
                rotationInput.x *= -1f;
            if (InvertY)
                rotationInput.y *= -1f;

            // Горизонтальное вращение по мировой оси Y
            Quaternion yawRotation = Quaternion.AngleAxis(rotationInput.x * RotationSpeed, Vector3.up);
            PlanarDirection = yawRotation * PlanarDirection;

            // Проецируем направление на горизонталь, чтобы убрать возможные отклонения
            PlanarDirection = Vector3.ProjectOnPlane(PlanarDirection, Vector3.up).normalized;

            // Вычисляем поворот по горизонтали
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, Vector3.up);

            // Обновляем вертикальный угол
            _targetVerticalAngle -= rotationInput.y * RotationSpeed;
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);

            // Поворот вверх/вниз по локальной оси X (right после yaw)
            Quaternion verticalRot = Quaternion.AngleAxis(_targetVerticalAngle, Vector3.right);

            // Собираем итоговую ориентацию
            Quaternion targetRotation = Quaternion.Slerp(
                transform.rotation,
                planarRot * verticalRot,
                1f - Mathf.Exp(-RotationSharpness * deltaTime)
            );

            transform.rotation = targetRotation;
        }

        private void CameraMovement(float deltaTime)
        {
            _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, FollowTransform.position, 1f - Mathf.Exp(-FollowingSharpness * deltaTime));
            transform.position = _currentFollowPosition;
        }
    }
}
