using Sources.Services.Factories;
using UnityEngine;
using Zenject;

namespace Sources.Behaviour.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private PlayerRoot _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        private IGameObjectsFactory _gameObjectsFactory;

        [Inject]
        private void Construct(IGameObjectsFactory gameObjectsFactory) => 
            _gameObjectsFactory = gameObjectsFactory;

        public void SpawnPlayer()
        {
            PlayerRoot player = _gameObjectsFactory.Create(_playerPrefab.gameObject, 
                _playerSpawnPoint.position, _playerSpawnPoint.rotation)
                .GetComponent<PlayerRoot>();
        }
    }
}