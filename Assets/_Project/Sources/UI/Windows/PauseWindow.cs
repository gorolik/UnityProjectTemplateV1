using Sources.Services.Scene;
using Sources.UI.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private int _menuSceneIndex = 1;
        
        private float _timeScaleBeforePause = 1;
        
        private ISceneService _sceneService;

        [Inject]
        private void Construct(ISceneService sceneService) => 
            _sceneService = sceneService;

        protected override void Init()
        {
            _timeScaleBeforePause = Time.timeScale;
            Time.timeScale = 0;
        }

        protected override void SubscribeUpdates()
        {
            _toMenuButton.onClick.AddListener(OnToMenuButtonClicked);
        }

        protected override void Cleanup()
        {
            Time.timeScale = _timeScaleBeforePause;
            
            _toMenuButton.onClick.RemoveListener(OnToMenuButtonClicked);
        }

        private void OnToMenuButtonClicked() => 
            _sceneService.Load(_menuSceneIndex, true);
    }
}