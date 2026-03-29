using System;
using System.Collections.Generic;
using Sources.UI.Windows;
using Sources.UI.WindowsSystem;

namespace Sources.Services.Factories
{
    public interface IUIFactory
    {
        public IReadOnlyList<WindowBase> OpenedWindows { get; }
        public T CreateWindow<T>(WindowId windowId, WindowBase windowToEnableAfterClose = null) where T : WindowBase;
        public void CreateWindow(WindowId windowId);
        public Curtain CreateCurtain();
        public event Action<WindowBase> OnWindowClosedEvent;
        public event Action<WindowBase> OnWindowEnabledEvent;
    }
}
