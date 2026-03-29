using System;
using Sources.Infrastructure;
using UnityEngine;

namespace BaseSettings.Scripts.Data
{
    public class SettingsDataInitializer : MonoBehaviour
    {
        [ContextMenu("Delete Settings Data")]
        private void DeleteSettingsData() => 
            SettingsDataLoader.ClearData();

        public event Action DataInitialized;

        public void Initialize()
        {
            try
            {
                GameData.GameDataContent.PlayerSettingsData = SettingsDataLoader.ReadData();
                
                if (GameData.GameDataContent.PlayerSettingsData == null)
                    throw new Exception("Null player settings data");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Data is incompatible or missing. Settings will be reset: " + e);
                
                GameData.GameDataContent.PlayerSettingsData = new PlayerSettingsData();
                SettingsDataLoader.SaveData(GameData.GameDataContent.PlayerSettingsData);
            }
            
            DataInitialized?.Invoke();
            Debug.Log("Settings data loaded:\n" + JsonUtility.ToJson(GameData.GameDataContent.PlayerSettingsData));
        }
    }
}