//using Unity.Netcode;

using UnityEngine;
using Zenject;

namespace Sources.Services.Factories
{
    public interface IGameObjectsFactory
    {
        void Tick(float deltaTime);
        void RegisterObject(GameObject existsGameObject);

        /// <summary>
        /// Создаёт объект с помощью Zenject, используя заданный контейнер
        /// </summary>
        GameObject Create(GameObject gameObjectPrefab, Vector3 position, Quaternion rotation,
            DiContainer container, Transform parent = null);

        /// <summary>
        /// Создаёт объект с помощью Zenject, используя глобальный контейнер
        /// </summary>
        GameObject Create(GameObject gameObjectPrefab, Vector3 position, Quaternion rotation, Transform parent = null);

        /*
        /// <summary>
        /// Создаёт объект с помощью Zenject, спавнит по сети, а затем задаёт parent по сети, если такой указан
        /// </summary>
        NetworkObject NetworkCreate(NetworkObject gameObjectPrefab, Vector3 position, Quaternion rotation, GameObjectsFactory.NetCreateType createType, ulong ownerId, NetworkObject parent = null);
        */
    }
}