using UnityEngine;
using TMPro;
using Zenject;
using Gameplay.Services;

namespace Gameplay.Views
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject[] _stars;

        private GameManager _gameManager;
        private int _starsCount;

        [Inject]
        public void Construct(Transform parentTransform, float timeSpent, int starsCount, GameManager gameManager)
        {
            _gameManager = gameManager;

            var minutes = Mathf.FloorToInt(timeSpent / 60f);
            var seconds = Mathf.FloorToInt(timeSpent % 60f);
            
            _timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            _starsCount = starsCount;
            transform.SetParent(parentTransform, false);
        }
        
        public void LoadMainMenu()
        {
            _gameManager.LoadMainMenu();
        }

        public class Factory : PlaceholderFactory<Transform, float, int, WinMenu>
        {
        }
    }   
}