using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Gameplay.Views;
using Gameplay.UI;
using Services;

namespace Gameplay.Services
{
    public class GameManager : MonoBehaviour
    {
        private const int MatchCountMultiplier = 3;
        
        [SerializeField] private GameObject _spawnFieldTransform;
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private List<Sprite> _goalIcons;
        [SerializeField] private int _maxCountModifier;

        private readonly List<CollectableItem> _collectableItems = new();
        private List<GoalView> _spawnObjectList = new();

        private GoalsController _goalsController;
        private CollectController _collectController;
        private SceneNavigation _sceneNavigation;
        
        private GameTimer _gameTimer;
        
        private List<int> _originalCounts = new();
        private List<Sprite> _originalSprites = new();

        private GoalView.Factory _goalViewFactory;
        private CollectableItem.Factory _collectableItemFactory;
        private WinMenu.Factory _winMenuFactory;
        private FailMenu.Factory _failMenuFactory;
        
        public GameState CurrentGameState { get; private set; }

        [Inject]
        public void Construct(CollectableItem.Factory collectableItemFactory, CollectController collectController,
            GoalsController goalsController, GameTimer gameTimer, WinMenu.Factory winMenuFactory,
            FailMenu.Factory failMenuFactory, GoalView.Factory goalViewFactory, SceneNavigation sceneNavigation)
        {
            _collectableItemFactory = collectableItemFactory;
            _goalsController = goalsController;
            _collectController = collectController;
            _gameTimer = gameTimer;
            _winMenuFactory = winMenuFactory;
            _failMenuFactory = failMenuFactory;
            _goalViewFactory = goalViewFactory;
            _sceneNavigation = sceneNavigation;

            _goalsController.GotWin += OnWin;
            _collectController.GotLoss += OnLose;
            _gameTimer.TimeIsOver += OnLose;

            SetUpGoals();
        }

        private void SetUpGoals()
        {
            var goalIndexes = Enumerable.Range(0, _goalIcons.Count).ToList();
            goalIndexes.Shuffle();

            for (int i = 0; i < _goalIcons.Count; i++)
            {
                var index = goalIndexes[i];
                var sprite = _goalIcons[index];
                var goalCount = Random.Range(1, _maxCountModifier) * MatchCountMultiplier;
                var newGoal = _goalViewFactory.Create(sprite, goalCount);
                
                _originalSprites.Add(sprite);
                _originalCounts.Add(goalCount);

                _spawnObjectList.Add(newGoal);
            }
            
            SetUpItems();
        }


        private void SetUpItems()
        {
            foreach (var element in _spawnObjectList)
            {
                for (int i = 0; i < element.GoalCount; ++i)
                {
                    var item = _collectableItemFactory.Create(element.Id, _spawnFieldTransform.transform);
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
            _goalsController.ClearGoalsForReplay();

            foreach (var item in _collectableItems)
            {
                Destroy(item.gameObject);
            }
            _collectableItems.Clear();

            foreach (var item in _spawnObjectList)
            {
                Destroy(item.gameObject);
            }
            _spawnObjectList.Clear();

            _collectController.ClearSlots();
            
            for (int i = 0; i < _goalIcons.Count; i++)
            {
                var newGoal = _goalViewFactory.Create(_originalSprites[i], _originalCounts[i]);
                _spawnObjectList.Add(newGoal);
            }
            
            SetUpItems();
        }

        public void LoadMainMenu()
        {
            _sceneNavigation.LoadMainMenu();
        }
    }
}