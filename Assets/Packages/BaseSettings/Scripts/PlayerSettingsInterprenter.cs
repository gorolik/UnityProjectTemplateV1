using System;
using Sources.Infrastructure;
using UnityEngine;
using UnityEngine.Audio;

namespace BaseSettings.Scripts
{
    public class PlayerSettingsInterpreter : MonoBehaviour
    {
        public static readonly int[] FrameRates = new int[]
        {
            30, 60, 120, 144, 165, 240
        };

        private const string _soundsParameter = "SoundsVolume";
        private const string _musicParameter = "MusicVolume";
        
        private const float _volumeMultiplier = 40f;

        [SerializeField] private bool _interpretOnStart = true;
        [SerializeField] private AudioMixer _audioMixer;
        
        public delegate void InterpretedDelegate();
        public static event InterpretedDelegate SettingsInterpretedStatic;
        
        public event Action SettingsInterpreted;

        public bool InterpretOnStart => _interpretOnStart;

        private void Start()
        {
            if (_interpretOnStart)
                ApplySettingsFromData();
        }

        public void ApplySettingsFromData()
        {
            var data = GameData.GameDataContent.PlayerSettingsData;
            
            if (data == null)
            {
                Debug.LogError("Player settings data is missing, applying skipped");
                return;
            }
            
            if (_audioMixer != null)
            {
                var soundsVolumeValue = Mathf.Log10(data.SoundsVolume) * _volumeMultiplier;
                _audioMixer.SetFloat(_soundsParameter, soundsVolumeValue);
                
                var musicVolumeValue = Mathf.Log10(data.MusicVolume) * _volumeMultiplier;
                _audioMixer.SetFloat(_musicParameter, musicVolumeValue);
            }
            else
                Debug.LogError("Audio Mixer is missing for master volume setting");
            
            QualitySettings.SetQualityLevel(data.Graphics);

#if UNITY_STANDALONE
            Resolution resolution = Screen.resolutions[data.Resolution];
            Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)data.ScreenMode);
#endif
            
            QualitySettings.vSyncCount = data.VerticalSync;

            Application.targetFrameRate = FrameRates[data.MaxFramesPerSecond];
            
            SettingsInterpreted?.Invoke();
            SettingsInterpretedStatic?.Invoke();
        }
    }
}