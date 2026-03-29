using System;
using System.Collections.Generic;
using Sources.UI.WindowsSystem;

namespace Sources.Services.Windows
{
    public interface IWindowService
    {
        public IReadOnlyList<WindowBase> OpenedWindows { get; }
        public T Open<T>(WindowId windowId, WindowBase windowToEnableAfterClose = null) where T : WindowBase;
        public void Open(WindowId windowId, WindowBase windowToEnableAfterClose = null);
        public event Action<WindowBase> OnWindowEnabled;
        public event Action<WindowBase> OnWindowClosed;
    }
}