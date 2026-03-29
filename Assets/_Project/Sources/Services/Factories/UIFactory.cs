using System;
using System.Collections.Generic;
using Sources.Data.ScriptableObjects.UI;
using Sources.UI.Windows;
using Sources.UI.WindowsSystem;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Sources.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly List<WindowBase> _openedWindows = new();
        
        private Transform _uiRoot;
        private WindowsList _windowsList;
        private DiContainer _container;

        public event Action<WindowBase> OnWindowClosedEvent;
        public event Action<WindowBase> OnWindowEnabledEvent;

        public IReadOnlyList<WindowBase> OpenedWindows => _openedWindows;

        public UIFactory(DiContainer container, WindowsList windowsList)
        {
            _container = container;
            _windowsList = windowsList;
        }

        public T CreateWindow<T>(WindowId windowId, WindowBase windowToEnableAfterClose = null) 
            where T : WindowBase
        {
            WindowBase window = InstantiateByWindowId(windowId);
            
            window.SetWindowId(windowId);
            window.SetWindowToEnableAfterClose(windowToEnableAfterClose);
            
            window.OnClosed += OnWindowClosed;
            window.OnEnabled += OnWindowEnabled;
            OnWindowEnabled(window);
            _openedWindows.Add(window);
            
            T tWindow = window as T;
            return tWindow;
        }

        public Curtain CreateCurtain()
        {
            Curtain curtain = Object.Instantiate(_windowsList.CurtainPrefab);
            
            Object.DontDestroyOnLoad(curtain.gameObject);
            curtain.HideImmediately();

            return curtain;
        }

        public void CreateWindow(WindowId windowId) => 
            CreateWindow<WindowBase>(windowId);

        private void CreateUIRoot()
        {
            if (_uiRoot)
            {
                Debug.LogError("UI Root already exists");
                return;
            }

            Transform uiRoot = Object.Instantiate(_windowsList.UIRootPrefab);
            _uiRoot = uiRoot;
        }

        private WindowBase InstantiateByWindowId(WindowId id)
        {
            WindowBase windowPrefab = _windowsList.GetPrefabById(id);

            if (!windowPrefab)
                throw new Exception("Unknown window type: " + id);
            
            if (!_uiRoot)
                CreateUIRoot();

            var instance = _container.InstantiatePrefabForComponent<WindowBase>
                (_windowsList.GetPrefabById(id), _uiRoot);
            
            return instance;
        }

        private void OnWindowClosed(WindowBase closedWindow)
        {
            closedWindow.OnClosed -= OnWindowClosed;
            closedWindow.OnEnabled -= OnWindowEnabled;
            
            _openedWindows.Remove(closedWindow);
            
            OnWindowClosedEvent?.Invoke(closedWindow);
        }

        private void OnWindowEnabled(WindowBase enabledWindow) => 
            OnWindowEnabledEvent?.Invoke(enabledWindow);
    }
}
