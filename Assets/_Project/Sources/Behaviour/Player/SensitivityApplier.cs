using System;
using BaseSettings.Scripts;
using PlayerController.Scripts.Implementation;
using Sources.Infrastructure;
using UnityEngine;

namespace Sources.Behaviour.Player
{
    public class SensitivityApplier : MonoBehaviour
    {
        [SerializeField] private BaseCharacterCamera _characterCamera;

        private void OnEnable()
        {
            OnPlayerSettingsInterpreted();
            PlayerSettingsInterpreter.SettingsInterpretedStatic += OnPlayerSettingsInterpreted;
        }

        private void OnDisable() => 
            PlayerSettingsInterpreter.SettingsInterpretedStatic -= OnPlayerSettingsInterpreted;

        private void OnPlayerSettingsInterpreted()
        {
            try
            {
                _characterCamera.RotationSpeed = GameData.GameDataContent.PlayerSettingsData.MouseSensitivity;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Cant apply mouse sensitivity: " + e);
            }
        }
    }
}