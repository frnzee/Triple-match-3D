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

        private GameManager _gameManager;

        public float GameTime { get; private set; }

        public event Action TimeIsOver;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            GameTime = TotalTime;
        }

        public void ResetTimer()
        {
            GameTime = TotalTime;
        }

        private void Update()
        {
            if (GameTime > 1 && _gameManager.CurrentGameState == GameState.Game)
            {
                GameTime -= Time.deltaTime;

                var minutes = Mathf.FloorToInt(GameTime / TimeDivider);
                var seconds = Mathf.FloorToInt(GameTime % TimeDivider);

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