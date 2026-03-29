using Sources.Data.ScriptableObjects;
using Sources.Services.Factories;
using Sources.Utilities;
using UnityEngine;
using Zenject;

namespace Sources.Infrastructure
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private CursorLocker _cursorLocker;
        
        private GlobalGameSettings _globalGameSettings;

        [Inject]
        private void Construct(GlobalGameSettings globalGameSettings) => 
            _globalGameSettings = globalGameSettings;

        public override void InstallBindings()
        {
            Container.Bind<IGameObjectsFactory>().To<GameObjectsFactory>()
                .FromNew().AsSingle().WithArguments(_globalGameSettings.DelayedTickTime);

            Container.Bind<CursorLocker>().FromInstance(_cursorLocker).AsSingle();
        }
    }
}