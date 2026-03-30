using Sources.Data.ScriptableObjects;
using Sources.Data.ScriptableObjects.UI;
using Sources.Services.Factories;
using Sources.Services.Input;
using Sources.Services.Scene;
using Sources.Services.Windows;
using Sources.UI.Utils.Sounds;
using UnityEngine;
using Zenject;

namespace Sources.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GlobalGameSettings _globalGameSettings;
        [SerializeField] private WindowsList _windowsList;
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private UISoundsSource _uiSoundsSource;
        
        public override void InstallBindings()
        {
            Container.Bind<GlobalGameSettings>().FromInstance(_globalGameSettings).AsSingle();
            Container.Bind<WindowsList>().FromInstance(_windowsList).AsSingle();
            
            Container.Bind<ISceneService>().To<SceneService>().FromNew().AsSingle();
            Container.Bind<IInputService>().To<KeyboardMouseInputService>().FromNew().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().FromNew().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().FromNew().AsSingle();
            
            Container.Bind<UISoundsSource>().FromInstance(_uiSoundsSource).AsSingle();
            Container.Bind<CoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();
        }
    }
}