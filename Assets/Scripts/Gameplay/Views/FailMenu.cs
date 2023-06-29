using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.Views
{
    public class FailMenu : MonoBehaviour
    {
        private GameManager _gameManager;

        [Inject]
        public void Construct(Transform parentTransform, GameManager gameManager)
        {
            _gameManager = gameManager;
            transform.SetParent(parentTransform, false);
        }

        public void LoadMainMenu()
        {
            _gameManager.LoadMainMenu();
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