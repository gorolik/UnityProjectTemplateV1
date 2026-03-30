using BaseSettings.Scripts;
using BaseSettings.Scripts.Data;
using Sources.Data;
using Sources.Services.Data;
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
        private IDataService _dataService;

        [Inject]
        private void Construct(ISceneService sceneService, IDataService dataService)
        {
            _sceneService = sceneService;
            _dataService = dataService;
        }

        private void Start()
        {
            LoadPlayerData();
            _settingsDataInitializer.Initialize();
            _playerSettingsInterpreter.ApplySettingsFromData();

            LoadGame();
        }

        private void LoadPlayerData()
        {
            if (_dataService.TryLoad(out PlayerData playerData))
                GameData.GameDataContent.PlayerData = playerData;
            else
            {
                PlayerData newData = new PlayerData();
                newData.TestIntValue = 31;
                
                GameData.GameDataContent.PlayerData = newData;
                _dataService.Save(newData);
                
                Debug.LogWarning("Cant load playerData, creating new data");
            }
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