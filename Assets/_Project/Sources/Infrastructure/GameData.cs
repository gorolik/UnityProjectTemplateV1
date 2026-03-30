using BaseSettings.Scripts.Data;
using Sources.Data;
using UnityEngine;

namespace Sources.Infrastructure
{
    public class GameData : MonoBehaviour
    {
        public static GameData Instance;
        public static GameDataContent GameDataContent = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
            GameDataContent = new();
        }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public class GameDataContent
    {
        public PlayerData PlayerData;
        public PlayerSettingsData PlayerSettingsData;
        public int ReturnSceneIndex;
    }
}