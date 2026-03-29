using System;
using TMPro;
using UnityEngine;

namespace BaseSettings.Scripts.SettingsElements
{
    public class DropDownSettingsElement : SettingsElement
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        public TMP_Dropdown Dropdown => _dropdown;
        public int Value => _dropdown.value;
        
        private void Awake()
        {
            _dropdown.onValueChanged.AddListener(delegate
            {
                TriggerValueChangedEvent();
            });
        }

        private void OnDestroy() => 
            _dropdown.onValueChanged.RemoveAllListeners();
        
        public void SetValue(int value)
        {
            if (value < 0 || value >= _dropdown.options.Count)
                throw new ArgumentException("Value must be positive and less than options count");

            _dropdown.SetValueWithoutNotify(value);
        }
    }
}