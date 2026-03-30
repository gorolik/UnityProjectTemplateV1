using System;
using Sources.Data;
using UnityEngine;

namespace Sources.Services.Data
{
    public class PlayerPrefsDataService : IDataService
    {
        private const string Key = "PlayerData";
        
        public void Save(PlayerData playerData)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(playerData));
            PlayerPrefs.Save();
        }

        public bool TryLoad(out PlayerData playerData)
        {
            try
            {
                if (!PlayerPrefs.HasKey(Key))
                    throw new Exception("Key: " + Key + " does not registered in player prefs");
                
                playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(Key));

                if (playerData == null)
                    throw new Exception("Loaded playerData is null");
                
                return true;
            }
            catch (Exception e)
            {
                playerData = null;
                return false;
            }
        }

        public void Clear() => 
            PlayerPrefs.DeleteAll();
    }
}