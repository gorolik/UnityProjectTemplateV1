using Sources.Services.Scene;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace Sources.Infrastructure
{
    public class ToInitSceneReturner : MonoBehaviour  
    {
        [SerializeField] private bool _returnToThisScene = true; 
        
        private ISceneService _sceneService;

        [Inject]
        private void Construct(ISceneService sceneService) => 
            _sceneService = sceneService;

        private void Awake()
        {
            if (GameData.Instance == null)
            {
                if (_returnToThisScene) 
                    GameData.GameDataContent.ReturnSceneIndex = SceneManager
                        .GetActiveScene()
                        .buildIndex;
                
                _sceneService.LoadImmediately(0);
            }
        }
    }
}