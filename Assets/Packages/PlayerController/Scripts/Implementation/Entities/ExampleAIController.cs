using PlayerController.Scripts.Implementation.Model;
using UnityEngine;

namespace PlayerController.Scripts.Implementation.Entities
{
    public class ExampleAIController : KinematicEntity
    {
        public float MovementPeriod = 1f;

        private void Update()
        {
            AICharacterInputs inputs = new AICharacterInputs();
            
            inputs.MoveVector = Mathf.Sin(Time.time * MovementPeriod) * Vector3.forward;
            inputs.LookVector = Vector3.Slerp(-Vector3.forward, Vector3.forward, inputs.MoveVector.z).normalized;

            Character.SetInputs(ref inputs);
        }
    }
}