using System;
using UnityEngine;

namespace BaseSettings.Scripts.SettingsElements
{
    public abstract class SettingsElement : MonoBehaviour
    {
        public event Action<SettingsElement> OnValueChanged;

        protected void TriggerValueChangedEvent() => 
            OnValueChanged?.Invoke(this);
    }
}