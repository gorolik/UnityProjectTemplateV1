using System;
using UnityEngine;

namespace BaseSettings.Scripts.Data
{
    public static class SettingsDataLoader
    {
        private const string _settingsKey = "settings";
        
        public static PlayerSettingsData ReadData()
        {
            if (!PlayerPrefs.HasKey(_settingsKey))
                throw new Exception("No data to read");
            
            return JsonUtility.FromJson<PlayerSettingsData>(PlayerPrefs.GetString(_settingsKey));
        }

        public static void SaveData(PlayerSettingsData data) => 
            PlayerPrefs.SetString(_settingsKey, JsonUtility.ToJson(data));

        public static void ClearData()
        {
            PlayerPrefs.DeleteKey(_settingsKey);
            PlayerPrefs.Save();
        }
    }
}