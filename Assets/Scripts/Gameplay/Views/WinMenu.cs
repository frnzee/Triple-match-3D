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
        private float _timeSpent;
        private int _starsCount;

        [Inject]
        public void Construct(Transform parentTransform, float timeSpent, int starsCount, GameManager gameManager)
        {
            _gameManager = gameManager;
            _timeSpent = timeSpent;
            _starsCount = starsCount;
            transform.SetParent(parentTransform, false);
        }

        private void Start()
        {
            _timeText.text = "4:19";
        }

        public class Factory : PlaceholderFactory<Transform, float, int, WinMenu>
        {
        }
    }   
}