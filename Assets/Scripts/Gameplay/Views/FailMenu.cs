using Gameplay.Services;
using Services;
using UnityEngine;
using Zenject;

namespace Gameplay.Views
{
    public class FailMenu : MonoBehaviour
    {
        private GameManager _gameManager;
        private SceneNavigation _sceneNavigation;
        
        [Inject]
        public void Construct(Transform parentTransform, GameManager gameManager, SceneNavigation sceneNavigation)
        {
            _gameManager = gameManager;
            _sceneNavigation = sceneNavigation;
            transform.SetParent(parentTransform, false);
        }

        public void LoadMainMenu()
        {
            _sceneNavigation.LoadMainMenu();
        }

        public void ReplayLevel()
        {
            _gameManager.ReplayLevel();
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<Transform, FailMenu>
        {
        }
    }
}