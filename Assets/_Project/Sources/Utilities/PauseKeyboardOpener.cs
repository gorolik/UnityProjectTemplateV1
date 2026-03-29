using System;
using System.Linq;
using Sources.Services.Input;
using Sources.Services.Windows;
using Sources.UI.WindowsSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Sources.Utilities
{
    public class PauseKeyboardOpener : MonoBehaviour
    {
        private IInputService _inputService;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IInputService inputService, IWindowService windowService)
        {
            _inputService = inputService;
            _windowService = windowService;
        }

        private void OnEnable() => 
            _inputService.InputActions.UI.Cancel.performed += OnKeyPressed;

        private void OnDisable() => 
            _inputService.InputActions.UI.Cancel.performed -= OnKeyPressed;

        private void OnKeyPressed(InputAction.CallbackContext context)
        {
            if (_windowService.OpenedWindows.All(x => x.WindowId != WindowId.Pause))
                _windowService.Open(WindowId.Pause);
        }
    }
}