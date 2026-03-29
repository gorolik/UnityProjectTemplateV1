using System;
using UnityEngine;

namespace Sources.UI.WindowsSystem
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField] private bool _shouldUnlockCursor = true;
        [Space]
        
        private WindowId _windowId;
        
        /// <summary>
        /// Окно (объект), который нужно активировать после закрытия текущего окна.
        /// Может быть null.
        /// </summary>
        private WindowBase _payload;
        
        public Action<WindowBase> OnClosed;
        public Action<WindowBase> OnEnabled;

        public WindowId WindowId => _windowId;
        public bool ShouldUnlockCursor => _shouldUnlockCursor;

        private void Start()
        {
            Init();
            SubscribeUpdates();
        }

        private void OnDestroy()
        {
            Cleanup();
            OnClosed?.Invoke(this);
        }

        private void OnEnable() => 
            OnEnabled?.Invoke(this);

        public void Close()
        {
            if (_payload)
                _payload.gameObject.SetActive(true);
            
            Destroy(gameObject);
        }

        public void SetWindowId(WindowId windowId) => 
            _windowId = windowId;

        public void SetWindowToEnableAfterClose(WindowBase payload)
        {
            _payload = payload;
            
            if (_payload)
                _payload.gameObject.SetActive(false);
        }

        protected virtual void Init() {}
        protected virtual void SubscribeUpdates() {}
        protected virtual void Cleanup() {}
    }
}