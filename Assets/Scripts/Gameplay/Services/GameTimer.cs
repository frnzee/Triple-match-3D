using System;
using UnityEngine;
using TMPro;
using Zenject;

namespace Gameplay.Services
{
    public class GameTimer : MonoBehaviour
    {
        private const float TotalTime = 300f;
        private const float TimeDivider = 60f;

        [SerializeField] private TextMeshProUGUI _timerText;

        private float _currentTime;
        private GameManager _gameManager;

        public float GameTime => _currentTime;
        public event Action TimeIsOver;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            _currentTime = TotalTime;
        }

        public void ResetTimer()
        {
            _currentTime = TotalTime;
        }

        private void Update()
        {
            if (_currentTime > 1 && _gameManager.CurrentGameState == GameState.Game)
            {
                _currentTime -= Time.deltaTime;

                var minutes = Mathf.FloorToInt(_currentTime / TimeDivider);
                var seconds = Mathf.FloorToInt(_currentTime % TimeDivider);

                var timeText = $"{minutes:0}:{seconds:00}";

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