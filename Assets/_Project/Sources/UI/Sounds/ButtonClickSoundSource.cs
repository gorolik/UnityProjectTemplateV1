using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.UI.Sounds
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickSoundSource : MonoBehaviour
    {
        private Button _button;
        private UISoundsSource _uiSoundsSource;

        [Inject]
        private void Construct(UISoundsSource uiSoundsSource) => 
            _uiSoundsSource = uiSoundsSource;

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            if (_uiSoundsSource)
                _uiSoundsSource.PlayButtonClickSound();
            else
                Debug.Log("uiSoundsSource is null, cant play press button sound");
        }
    }
}