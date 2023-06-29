using System;
using UnityEngine;
using TMPro;
using Zenject;

namespace Gameplay.Services
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        private float _totalTime = 300f;
        private float _currentTime;
        public float GameTime => _currentTime;
        private GameManager _gameManager;
        public event Action TimeIsOver;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            _currentTime = _totalTime;
        }

        public void ResetTimer()
        {
            _currentTime = _totalTime;
        }

        private void Update()
        {
            if (_currentTime > 1 && _gameManager.CurrentGameState == GameState.Game)
            {
                _currentTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(_currentTime / 60f);
                int seconds = Mathf.FloorToInt(_currentTime % 60f);

                string timeText = string.Format("{0:0}:{1:00}", minutes, seconds);

                _timerText.text = timeText;
                return;
            }

            if (_gameManager.CurrentGameState == GameState.Game)
            {
                TimeIsOver?.Invoke();
            }
        }
    }
}