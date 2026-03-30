using DG.Tweening;
using UnityEngine;

namespace Sources.UI.Utils
{
    public class ButtonCursorAnimation : MonoBehaviour
    {
        [Header("Hover")]
        [SerializeField] private float _hoverTargetScale = 1.1f;
        [SerializeField] private float _hoverDuration = 0.25f;
        [Header("Click")]
        [SerializeField] private float _clickTargetScale = 0.9f;
        [SerializeField] private float _clickDuration = 0.15f;
        
        public void OnPointerEnter() => 
            transform
                .DOScale(Vector3.one * _hoverTargetScale, _hoverDuration)
                .SetLink(gameObject)
                .SetUpdate(UpdateType.Normal, true);

        public void OnPointerExit() => 
            transform
                .DOScale(Vector3.one, _hoverDuration)
                .SetLink(gameObject) 
                .SetUpdate(UpdateType.Normal, true);

        public void OnPointerDown() => 
            transform
                .DOScale(Vector3.one * _clickTargetScale, _clickDuration)
                .SetLink(gameObject)
                .SetUpdate(UpdateType.Normal, true);

        public void OnPointerUp() => 
            transform
                .DOScale(Vector3.one, _clickDuration)
                .SetLink(gameObject)
                .SetUpdate(UpdateType.Normal, true);
    }
}
