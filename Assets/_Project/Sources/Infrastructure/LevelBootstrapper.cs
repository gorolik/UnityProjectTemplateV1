using Sources.Behaviour.Player;
using Sources.Services.Factories;
using UnityEngine;
using Zenject;

namespace Sources.Infrastructure
{
    public class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private PlayerFactory _playerFactory;
        
        private IGameObjectsFactory _gameObjectsFactory;

        [Inject]
        private void Construct(IGameObjectsFactory gameObjectsFactory)
        {
            _gameObjectsFactory = gameObjectsFactory;
        }
        
        private void Start()
        {
            _playerFactory.SpawnPlayer();
        }

        private void Update() => 
            _gameObjectsFactory.Tick(Time.deltaTime);
    }
}