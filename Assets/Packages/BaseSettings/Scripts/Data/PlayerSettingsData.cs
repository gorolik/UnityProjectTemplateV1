using System;
using UnityEngine;

namespace BaseSettings.Scripts.Data
{
    [Serializable]
    public class PlayerSettingsData
    {
        public float SoundsVolume = 1;
        public float MusicVolume = 1;
        public float MouseSensitivity = 0.5f;
        public int Graphics = GetGraphicsByPlatform();
        public int ScreenMode = 1;
        public int Resolution = Screen.resolutions.Length - 1;

        public int VerticalSync = 0;

        public int MaxFramesPerSecond = GetOptimalTargetFPS();

        // Можно добавить проверку с пк ли запускается, чтобы даже в браузере была хорошая графика
        private static int GetGraphicsByPlatform()
        {
            if (Application.isMobilePlatform)
                return 1;
            else
                return 0;
        }

        private static int GetOptimalTargetFPS()
        {
            for (int i = PlayerSettingsInterpreter.FrameRates.Length - 1; i >= 0; i--)
            {
                var refreshRate = Screen.currentResolution.refreshRateRatio;
                if (PlayerSettingsInterpreter.FrameRates[i] <= refreshRate.value + 1)
                    return i;
            }

            return PlayerSettingsInterpreter.FrameRates.Length - 1;
        }
    }
}