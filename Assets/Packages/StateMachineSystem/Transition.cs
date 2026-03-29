using UnityEngine;

namespace StateMachineSystem
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] private State _targetState;

        public State TargetState => _targetState;

        [HideInInspector] public bool ShouldTransit = false;

        private void Update() => 
            Tick(Time.deltaTime);

        protected abstract void Tick(float deltaTime);
    }
}
