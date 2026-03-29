using BaseSettings.Scripts;
using BaseSettings.Scripts.Data;
using Sources.Services.Scene;
using UnityEngine;
using Zenject;

namespace Sources.Infrastructure
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private SettingsDataInitializer _settingsDataInitializer;
        [SerializeField] private PlayerSettingsInterpreter _playerSettingsInterpreter;
        [SerializeField] private int _nextSceneIndex = 1;
        
        private ISceneService _sceneService;

        [Inject]
        private void Construct(ISceneService sceneService) => 
            _sceneService = sceneService;

        private void Start()
        {
            _settingsDataInitializer.Initialize();
            _playerSettingsInterpreter.ApplySettingsFromData();

            LoadGame();
        }

        private void LoadGame()
        {
            if (GameData.GameDataContent.ReturnSceneIndex != 0)
                _sceneService.Load(GameData.GameDataContent.ReturnSceneIndex, true);
            else
                _sceneService.Load(_nextSceneIndex, true);
        }
    }
}