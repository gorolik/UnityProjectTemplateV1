using System;
using Sources.UI.Windows;
using Sources.UI.WindowsSystem;
using UnityEngine;

namespace Sources.Data.ScriptableObjects.UI
{
    [CreateAssetMenu (menuName = "Scriptable Objects/UI/Windows list", fileName = "New windows list", order = 51)]
    public class WindowsList : ScriptableObject
    {
        [SerializeField] private Transform _uiRootPrefab;
        [SerializeField] private Curtain _curtainPrefab;
        [SerializeField] private Window[] _windows;

        public Transform UIRootPrefab => _uiRootPrefab;
        public Curtain CurtainPrefab => _curtainPrefab;

        public WindowBase GetPrefabById(WindowId windowId)
        {
            foreach (var window in _windows)
            {
                if (window.WindowId == windowId)
                    return window.WindowPrefab;
            }

            return null;
        }

        [Serializable]
        public struct Window
        {
            [SerializeField] private WindowBase _windowPrefab;
            [SerializeField] private WindowId _windowId;

            public WindowBase WindowPrefab => _windowPrefab;
            public WindowId WindowId => _windowId;
        }
    }
}