using UnityEngine;

namespace StateMachineSystem
{
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private Transition[] _transitions;

        public Transition[] Transitions => _transitions;

        private void Update() => 
            Tick(Time.deltaTime);

        protected abstract void Tick(float deltaTime);

        public void Enter()
        {
            gameObject.SetActive(true);

            foreach (Transition transition in _transitions) 
                transition.gameObject.SetActive(true);
        }
        
        public void Exit()
        {
            gameObject.SetActive(false);

            foreach (Transition transition in _transitions) 
                transition.gameObject.SetActive(false);
        }
    }
}
