using UnityEngine;

namespace StateMachineSystem
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State _initialState;
        
        private State _currentState;

        public void Init() => 
            EnterToState(_initialState);

        private void Update()
        {
            if (_currentState != null)
            {
                foreach (Transition transition in _currentState.Transitions)
                {
                    if (transition.ShouldTransit)
                    {
                        transition.ShouldTransit = false;
                        EnterToState(transition.TargetState);
                    }
                }
            }
        }

        public void EnterToState(State state)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = state;
            state.Enter();
        }
    }
}
