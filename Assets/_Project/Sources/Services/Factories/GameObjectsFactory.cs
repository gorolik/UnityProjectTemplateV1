using System.Collections.Generic;
using Sources.Infrastructure;
using UnityEngine;
using Zenject;

//using Unity.Netcode;

namespace Sources.Services.Factories
{
    public class GameObjectsFactory : IUpdatable, IGameObjectsFactory
    {
        private readonly float _delayedTickTime;
        private readonly List<IDelayedUpdatable> _delayedUpdates = new();
        private readonly DiContainer _container;
        
        private float _lastUpdateTime;

        public enum NetCreateType
        {
            JustSpawn,
            WithOwner,
            AsPlayer,
        }

        public GameObjectsFactory(DiContainer container, float delayedTickTime)
        {
            _container = container;
            _delayedTickTime = delayedTickTime;
            
            //NightPool.GameObjectInstantiated.AddListener(TryGetInterfaces);
        }

        ~GameObjectsFactory()
        {
            //NightPool.GameObjectInstantiated.RemoveListener(TryGetInterfaces);
        }
        
        public void Tick(float deltaTime) => 
            HandleDelayedUpdatables();

        public void RegisterObject(GameObject existsGameObject) => 
            TryGetInterfaces(existsGameObject);

        /// <summary>
        /// Создаёт объект с помощью Zenject, используя заданный контейнер
        /// </summary>
        public GameObject Create(GameObject gameObjectPrefab, Vector3 position, Quaternion rotation,
            DiContainer container, Transform parent = null)
        {
            GameObject newObject = Object.Instantiate(gameObjectPrefab, position, rotation, parent);
            RegisterCreatedObject(newObject, container);
            
            return newObject;
        }

        /// <summary>
        /// Создаёт объект с помощью Zenject, используя глобальный контейнер
        /// </summary>
        public GameObject Create(GameObject gameObjectPrefab, Vector3 position, Quaternion rotation, Transform parent = null) => 
            Create(gameObjectPrefab, position, rotation, _container, parent);

        /*
        /// <summary>
        /// Создаёт объект с помощью Zenject, спавнит по сети, а затем задаёт parent по сети, если такой указан
        /// </summary>
        public NetworkObject NetworkCreate(NetworkObject gameObjectPrefab, Vector3 position, Quaternion rotation, NetCreateType createType, ulong ownerId, NetworkObject parent = null)
        {
            GameObject newObject = Create(gameObjectPrefab.gameObject, position, rotation);
            NetworkObject newNetworkObject = newObject.GetComponent<NetworkObject>();

            switch (createType)
            {
                case NetCreateType.JustSpawn:
                    newNetworkObject.Spawn(true);
                    break;
                case NetCreateType.WithOwner:
                    newNetworkObject.SpawnWithOwnership(ownerId, true);
                    break;
                case NetCreateType.AsPlayer:
                    newNetworkObject.SpawnAsPlayerObject(ownerId, true);
                    break;
            }
            
            if (parent != null)
                newNetworkObject.TrySetParent(parent);

            return newNetworkObject;
        }
        */

        private void RegisterCreatedObject(GameObject createdGameObject, DiContainer container)
        {
            container.InjectGameObject(createdGameObject);
            RegisterObject(createdGameObject);
        }

        private void TryGetInterfaces(GameObject instantiatedGameObject)
        {
            IDelayedUpdatable[] delayedUpdates = instantiatedGameObject.GetComponentsInChildren<IDelayedUpdatable>(true);
            foreach (IDelayedUpdatable delayedUpdate in delayedUpdates) 
                _delayedUpdates.Add(delayedUpdate);
        }

        private void HandleDelayedUpdatables()
        {
            if (Time.time - _lastUpdateTime >= _delayedTickTime)
            {
                UpdateAll(Time.time - _lastUpdateTime);
                _lastUpdateTime = Time.time;
            }
        }

        private void UpdateAll(float deltaTime)
        {
            for (int i = _delayedUpdates.Count - 1; i >= 0; i--)
            {
                var delayedUpdate = _delayedUpdates[i] as UnityEngine.Object;

                if (delayedUpdate == null)
                {
                    _delayedUpdates.RemoveAt(i);
                    continue;
                }
                
                _delayedUpdates[i].DelayedTick(deltaTime);
            }
        }
    }
}