using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseSettings.Scripts.SettingsElements
{
    public class SliderSettingsElement : SettingsElement
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private float _minValue = 0;
        [SerializeField] private float _maxValue = 1;
        
        private float _value;

        public float Value => _value;

        private void Awake()
        {
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            
            _slider.onValueChanged.AddListener(delegate
            {
                ValueChangedBySlider(_slider.value); 
            });
            
            _inputField.onValueChanged.AddListener(delegate
            {
                if (float.TryParse(_inputField.text, out float newValue))
                {
                    ValueChangedByInput(newValue);
                }
            });
        }

        public void SetValue(float value)
        {
            ChangeValue(value);
            _slider.value = _value;
            _inputField.text = _value.ToString();
        }

        private void ChangeValue(float value) => 
            _value = Mathf.Clamp((float)Math.Round(value, 2), _minValue, _maxValue);

        private void ValueChangedBySlider(float newValue)
        {
            ChangeValue(newValue);
            _inputField.text = _value.ToString();
            TriggerValueChangedEvent();
        }

        private void ValueChangedByInput(float newValue)
        {
            ChangeValue(newValue);
            _slider.value = _value;
            TriggerValueChangedEvent();
        }
    }
}