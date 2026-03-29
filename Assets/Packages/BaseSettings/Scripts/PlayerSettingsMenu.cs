using System;
using System.Linq;
using BaseSettings.Scripts.Data;
using BaseSettings.Scripts.SettingsElements;
using Sources.Infrastructure;
using TMPro;
using UnityEngine;

namespace BaseSettings.Scripts
{
    public class PlayerSettingsMenu : MonoBehaviour
    {
        [SerializeField] private PlayerSettingsInterpreter _interpreter;
        [SerializeField] private SettingsElement[] _elements;
        [Header("General")]
        [SerializeField] private SliderSettingsElement _musicSetting;
        [SerializeField] private SliderSettingsElement _soundsSetting;
        [SerializeField] private SliderSettingsElement _mouseSensitivitySetting;
        [Header("Graphics")]
        [SerializeField] private DropDownSettingsElement _graphicsSettings;
        [SerializeField] private DropDownSettingsElement _screenModeSetting;
        [SerializeField] private DropDownSettingsElement _resolutionSetting;
        [SerializeField] private DropDownSettingsElement _verticalSyncSetting;
        [SerializeField] private DropDownSettingsElement _maxFramesPerSecondSetting;

        private void Start()
        {
            if (GameData.GameDataContent.PlayerSettingsData == null)
            {
                Debug.Log("Player settings is missing, menu init skipped");
                return;
            }
            
            ApplyPlatformSpecificSettings();
            InitElementValues();
            SetElementsValueFromData();
            SubscribeOnChanges();
        }

        private void OnDestroy() => 
            DescribeOnChanges();

        private void ApplyPlatformSpecificSettings()
        {
            RuntimePlatform platform = Application.platform;

            bool isDesktop = platform == RuntimePlatform.WindowsPlayer ||
                             platform == RuntimePlatform.OSXPlayer ||
                             platform == RuntimePlatform.LinuxPlayer ||
                             Application.isEditor;

            bool isMobile = platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
            bool isWebGL = platform == RuntimePlatform.WebGLPlayer;

            _screenModeSetting.gameObject.SetActive(isDesktop);
            _resolutionSetting.gameObject.SetActive(isDesktop);
            _verticalSyncSetting.gameObject.SetActive(true);       // не факт что будет работать на WebGL
            _maxFramesPerSecondSetting.gameObject.SetActive(true);
        }

        private void InitElementValues()
        {
            _graphicsSettings.Dropdown.options.Clear();
            string[] graphicNames = QualitySettings.names;
            _graphicsSettings.Dropdown.AddOptions(graphicNames.ToList());
            
            _screenModeSetting.Dropdown.options.Clear();
            string[] screenModes = Enum.GetNames(typeof(FullScreenMode));
            _screenModeSetting.Dropdown.AddOptions(screenModes.ToList());
            
            _resolutionSetting.Dropdown.options.Clear();
            Resolution[] resolutions = Screen.resolutions;
            foreach (Resolution resolution in resolutions)
                _resolutionSetting.Dropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + "x" + resolution.height));
            
            _verticalSyncSetting.Dropdown.options.Clear();
            _verticalSyncSetting.Dropdown.AddOptions(new string[]
            {
                "Disable",
                "Enable"
            }.ToList());

            _maxFramesPerSecondSetting.Dropdown.options.Clear();
            foreach (int frameRate in PlayerSettingsInterpreter.FrameRates) 
                _maxFramesPerSecondSetting.Dropdown.options.Add(new TMP_Dropdown.OptionData(frameRate.ToString()));
        }

        private void WriteDataFromElementsToContainer()
        {
            var playerSettingsData = GameData.GameDataContent.PlayerSettingsData;
            
            playerSettingsData.MusicVolume = _musicSetting.Value;
            playerSettingsData.SoundsVolume = _soundsSetting.Value;
            playerSettingsData.MouseSensitivity = _mouseSensitivitySetting.Value;
            playerSettingsData.Graphics = _graphicsSettings.Value;
            playerSettingsData.ScreenMode = _screenModeSetting.Value;
            playerSettingsData.Resolution = _resolutionSetting.Value;
            playerSettingsData.VerticalSync = _verticalSyncSetting.Value;
            playerSettingsData.MaxFramesPerSecond = _maxFramesPerSecondSetting.Value;
        }

        private void SetElementsValueFromData()
        {
            var playerSettingsData = GameData.GameDataContent.PlayerSettingsData;
            
            _musicSetting.SetValue(playerSettingsData.MusicVolume);
            _soundsSetting.SetValue(playerSettingsData.SoundsVolume);
            _mouseSensitivitySetting.SetValue(playerSettingsData.MouseSensitivity);
            _graphicsSettings.SetValue(playerSettingsData.Graphics);
            _screenModeSetting.SetValue(playerSettingsData.ScreenMode);
            _resolutionSetting.SetValue(playerSettingsData.Resolution);
            _verticalSyncSetting.SetValue(playerSettingsData.VerticalSync);
            _maxFramesPerSecondSetting.SetValue(playerSettingsData.MaxFramesPerSecond);
        }

        private void OnValueChanged(SettingsElement element)
        {
            WriteDataFromElementsToContainer();
            SettingsDataLoader.SaveData(GameData.GameDataContent.PlayerSettingsData);
            _interpreter.ApplySettingsFromData();
        }

        private void SubscribeOnChanges()
        {
            foreach (var element in _elements) 
                element.OnValueChanged += OnValueChanged;
        }

        private void DescribeOnChanges()
        {
            foreach (var element in _elements) 
                element.OnValueChanged -= OnValueChanged;
        }
    }
}
