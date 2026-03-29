using System;
using Sources.Services.Windows;
using Sources.UI.WindowsSystem;
using UnityEngine;
using Zenject;

namespace Sources.UI.MainMenu
{
    public class MainMenuBootstrapper : MonoBehaviour
    {
        private IWindowService _windowService;

        [Inject]
        private void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void Start()
        {
            _windowService.Open(WindowId.MainMenu);
        }
    }
}