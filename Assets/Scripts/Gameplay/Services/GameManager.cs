using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Gameplay.Views;
using Gameplay.UI;
using Services;
using UnityEngine.SceneManagement;

namespace Gameplay.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnFieldTransform;
        [SerializeField] private GameObject _mainCanvas;

        private CollectableItem.Factory _collectableItemFactory;
        private GoalsController _goalsController;
        private CollectController _collectController;
        private GameTimer _gameTimer;
        private WinMenu.Factory _winMenuFactory;
        private FailMenu.Factory _failMenuFactory;
        private List<GoalSlot> _goalSlots = new();
        private List<GoalSlot> _goalSlotsForLevelReplay = new();
        private List<CollectableItem> _collectableItems = new();
        private SceneNames _sceneNames;
        
        public GameState CurrentGameState { get; private set; }
        
        [Inject]
        public void Construct(CollectableItem.Factory collectableItemFactory, CollectController collectController,
            GoalsController goalsController, GameTimer gameTimer, WinMenu.Factory winMenuFactory,
            FailMenu.Factory failMenuFactory, SceneNames sceneNames)
        {
            _collectableItemFactory = collectableItemFactory;
            _goalsController = goalsController;
            _collectController = collectController;
            _gameTimer = gameTimer;
            _winMenuFactory = winMenuFactory;
            _failMenuFactory = failMenuFactory;
            _sceneNames = sceneNames;

            _goalsController.GotWin += OnWin;
            _collectController.GotLoss += OnLose;
            _gameTimer.TimeIsOver += OnLose;
        }

        public void SetUpItems(List<GoalSlot> goalSlots)
        {
            _goalSlots = goalSlots;
            _goalSlotsForLevelReplay = goalSlots;

            foreach (var goal in _goalSlots)
            {
                for (int i = 0; i < goal.GoalCount; ++i)
                {
                    var item = _collectableItemFactory.Create(goal.name, _spawnFieldTransform.transform);
                    _collectableItems.Add(item);
                }
            }
            
            CurrentGameState = GameState.Game;
            _gameTimer.ResetTimer();
        }

        private void OnWin()
        {
            CurrentGameState = GameState.Win;
            _winMenuFactory.Create(_mainCanvas.transform, _gameTimer.GameTime, 2);
            _goalsController.GotWin -= OnWin;
        }

        private void OnLose()
        {
            CurrentGameState = GameState.Lose;
            _failMenuFactory.Create(_mainCanvas.transform);
            _collectController.GotLoss -= OnLose;
            _gameTimer.TimeIsOver -= OnLose;
        }

        public void RemoveItem(CollectableItem itemToRemove)
        {
            _collectableItems.Remove(itemToRemove);
        }
        
        public void ReplayLevel()
        {
            _goalsController.ResetGoalsForReplay();
            
            foreach (var item in _collectableItems)
            {
                Destroy(item.gameObject);
            }
            _collectableItems.Clear();
            
            _collectController.ClearSlots();

            SetUpItems(_goalSlotsForLevelReplay);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_sceneNames.MainMenu);
        }
    }
}