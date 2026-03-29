using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Infrastructure;
using Sources.Services.Factories;
using Sources.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Services.Scene
{
    public class SceneService : ISceneService
    {
        private readonly CoroutineRunner _coroutineRunner;

        private Dictionary<int, AsyncOperation> _sceneLoadProgresses;
        private Curtain _curtain;

        public SceneService(CoroutineRunner coroutineRunner, IUIFactory uIFactory)
        {
            _coroutineRunner = coroutineRunner;
            _sceneLoadProgresses = new Dictionary<int, AsyncOperation>();
            
            _curtain = uIFactory.CreateCurtain();
        }

        public void Load(int sceneIndex, bool useCurtain, Action onLoaded)
        {
            Debug.Log("load scene: " + sceneIndex);
            
            if(useCurtain)
            {
                _curtain.Show(() =>
                {
                    _coroutineRunner.StartCoroutine(LoadScene(sceneIndex, true, useCurtain, onLoaded));
                });
                return;
            }

            _coroutineRunner.StartCoroutine(LoadScene(sceneIndex, true, useCurtain, onLoaded));
        }
        public void LoadImmediately(int sceneIndex, Action onLoad = null)
        {
            Debug.Log("load scene immediately: " + sceneIndex);
            SceneManager.LoadScene(sceneIndex);
            onLoad?.Invoke();
        }

        public void LoadAdditive(int sceneIndex, Action onLoad = null)
        {
            Debug.Log($"load additive scene: {sceneIndex}");
            var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

            if(onLoad != null)
            {
                asyncOperation.completed += _ => onLoad.Invoke();
            }
        }

        public void Unload(int sceneIndex, Action onUnload = null)
        {
            Debug.Log($"unload scene: {sceneIndex}");

            if (!IsSceneLoaded(sceneIndex))
                return;

            if (onUnload != null)
            {
                SceneManager.sceneUnloaded += OnSceneUnloaded;

                void OnSceneUnloaded(UnityEngine.SceneManagement.Scene sceneData)
                {
                    SceneManager.sceneUnloaded -= OnSceneUnloaded;

                    if (sceneData.buildIndex == sceneIndex)
                        onUnload.Invoke();
                }
            }
            SceneManager.UnloadSceneAsync(sceneIndex);
        }
        private IEnumerator LoadScene(int sceneIndex, bool activateScene, bool useCurtain, Action onLoaded)
        {
            AsyncOperation waitLoadScene = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            waitLoadScene.allowSceneActivation = activateScene;
            _sceneLoadProgresses.Add(sceneIndex, waitLoadScene);

            do
            {
                yield return null;
            }
            while (!waitLoadScene.isDone);

            onLoaded?.Invoke();

            if(activateScene)
            {
                _sceneLoadProgresses.Remove(sceneIndex);
            }
            if(useCurtain)
            {
                _curtain.Hide();
            }
        }

        private bool IsSceneLoaded(int idx)
            => SceneManager.GetSceneAt(idx).isLoaded;

        public static int GetActiveSceneIndex()
            => SceneManager.GetActiveScene().buildIndex;
    }
}