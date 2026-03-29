using System;
using System.Collections.Generic;
using Sources.Services.Factories;
using Sources.UI.WindowsSystem;
using UnityEngine;

namespace Sources.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public IReadOnlyList<WindowBase> OpenedWindows => _uiFactory.OpenedWindows;
        
        public event Action<WindowBase> OnWindowClosed
        {
            add => _uiFactory.OnWindowClosedEvent += value;
            remove => _uiFactory.OnWindowClosedEvent -= value;
        }
        
        public event Action<WindowBase> OnWindowEnabled
        {
            add => _uiFactory.OnWindowEnabledEvent += value;
            remove => _uiFactory.OnWindowEnabledEvent -= value;
        }

        public WindowService(IUIFactory uiFactory) => 
            _uiFactory = uiFactory;

        public void Open(WindowId id, WindowBase windowToEnableAfterClose = null) => 
            Open<WindowBase>(id, windowToEnableAfterClose);

        public T Open<T>(WindowId id, WindowBase windowToEnableAfterClose = null) where T : WindowBase
        {
            try
            {
                return _uiFactory.CreateWindow<T>(id, windowToEnableAfterClose);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}