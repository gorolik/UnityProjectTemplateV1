using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI.WindowsSystem
{
    [RequireComponent(typeof(Button))]
    public class CloseWindowButton : MonoBehaviour
    {
        [SerializeField] private WindowBase _windowToClose;
        
        private Button _button;

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable() =>
            _button.onClick.AddListener(CloseWindow);

        private void OnDisable() =>
            _button.onClick.RemoveListener(CloseWindow);

        private void CloseWindow() =>
            _windowToClose.Close();
    }
}