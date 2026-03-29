using UnityEngine;

namespace PlayerController.Scripts.Implementation.Entities
{
    /// <summary>
    /// Представляет класс для наследования верхних сущностей,
    /// упрощающий трансформацию персонажей,
    /// основанных на <see cref="BaseCharacterController"/>
    /// </summary>
    public abstract class KinematicEntity : MonoBehaviour
    {
        [SerializeField] private BaseCharacterController _character;
        
        public BaseCharacterController Character => _character;

        public Vector3 Position => _character.transform.position;
        public Quaternion Rotation => _character.transform.rotation;

        /// <summary>
        /// Переносит character в точку
        /// </summary>
        /// <param name="point"></param>
        public virtual void Teleport(Vector3 point) => 
            _character.Motor.SetPosition(point);

        /// <summary>
        /// Установить поворот character
        /// </summary>
        /// <param name="euler">в</param>
        public virtual void SetViewEuler(Vector3 euler)
        {
            Quaternion yaw = Quaternion.Euler(0f, euler.y, 0f);
            Quaternion pitch = Quaternion.Euler(euler.x, 0f, 0f);
            Quaternion finalRot = yaw * pitch;
            _character.Motor.SetRotation(finalRot);
        }
        
        /// <summary>
        /// Повернуть character в сторону точки
        /// </summary>
        /// <param name="lookPoint"></param>
        public virtual void SetViewToPoint(Vector3 lookPoint)
        {
            Vector3 direction = lookPoint - Position;
            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            Quaternion yaw = Quaternion.LookRotation(direction, Vector3.up);
            _character.Motor.SetRotation(yaw);
        }
    }
}