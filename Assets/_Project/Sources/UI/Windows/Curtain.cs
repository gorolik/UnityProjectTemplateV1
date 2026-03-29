using System;
using System.Collections;
using Sources.UI.WindowsSystem;
using UnityEngine;

namespace Sources.UI.Windows
{
    public class Curtain : WindowBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine _coroutine;

        public void HideImmediately() => 
            _canvasGroup.alpha = 0;

        public void Hide(Action onFullyHided = null)
        {
            gameObject.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(ChangeTransparentcyCoroutine(0f, onFullyHided, true));
        }

        public void Show(Action onFullyShown)
        {
            gameObject.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(ChangeTransparentcyCoroutine(1f, onFullyShown, false));
        }

        private IEnumerator ChangeTransparentcyCoroutine(float targetAlpha, Action onComplete, bool disableAtEnd)
        {
            while(_canvasGroup.alpha != targetAlpha)
            {
                float speed = 2.5f;

                if (Time.deltaTime > 0)
                    ChangeAlpha(speed * Time.deltaTime);
                else
                    ChangeAlpha(speed * Time.unscaledDeltaTime);
                
                yield return null;
            }
            
            onComplete?.Invoke();
            yield return null;
            _coroutine = null;

            if (disableAtEnd)
                gameObject.SetActive(false);

            void ChangeAlpha(float speed) 
                => _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, targetAlpha, speed);
        }
    }
}