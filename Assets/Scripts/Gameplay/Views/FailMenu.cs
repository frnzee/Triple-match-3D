using Gameplay.Services;
using Services;
using Services.Audio;
using UnityEngine;
using Zenject;

namespace Gameplay.Views
{
    public class FailMenu : MonoBehaviour
    {
        private GameManager _gameManager;
        private SceneNavigation _sceneNavigation;
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(Transform parentTransform, GameManager gameManager, SceneNavigation sceneNavigation, AudioManager audioManager)
        {
            _gameManager = gameManager;
            transform.SetParent(parentTransform, false);
            _sceneNavigation = sceneNavigation;
            _audioManager = audioManager;
        }

        public void LoadMainMenu()
        {
            _audioManager.Play("ButtonClick");
            _sceneNavigation.LoadMainMenu();
        }

        public void ReplayLevel()
        {
            _audioManager.Play("ButtonClick");
            _gameManager.ReplayLevel();
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<Transform, FailMenu>
        {
        }
    }
}