using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Gameplay.Views;
using Gameplay.UI;

namespace Gameplay.Services
{
    public class GameManager : MonoBehaviour
    {
        private const int MaxCountModifier = 2;

        [SerializeField] private GameObject _spawnFieldTransform;
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private List<Sprite> _goalIcons;

        private CollectableItem.Factory _collectableItemFactory;
        private GoalsController _goalsController;
        private CollectController _collectController;
        private GameTimer _gameTimer;
        private WinMenu.Factory _winMenuFactory;
        private FailMenu.Factory _failMenuFactory;
        private List<GoalView> _goals = new();
        private List<GoalView> _goalsForReplay = new();
        private List<CollectableItem> _collectableItems = new();
        private readonly List<GoalView> _spawnObjectList = new();
        private GoalView.Factory _goalSlotFactory;
        
        public GameState CurrentGameState { get; private set; }
        
        [Inject]
        public void Construct(CollectableItem.Factory collectableItemFactory, CollectController collectController,
            GoalsController goalsController, GameTimer gameTimer, WinMenu.Factory winMenuFactory,
            FailMenu.Factory failMenuFactory, GoalView.Factory goalSlotFactory)
        {
            _collectableItemFactory = collectableItemFactory;
            _goalsController = goalsController;
            _collectController = collectController;
            _gameTimer = gameTimer;
            _winMenuFactory = winMenuFactory;
            _failMenuFactory = failMenuFactory;
            _goalSlotFactory = goalSlotFactory;

            _goalsController.GotWin += OnWin;
            _collectController.GotLoss += OnLose;
            _gameTimer.TimeIsOver += OnLose;
            
            SetUpGoals();
        }

        private void SetUpGoals()
        {
            for (int i = 0; i < _goalIcons.Count; ++i)
            {
                var randomIndex = Random.Range(0, _goalIcons.Count);
                var sprite = _goalIcons[randomIndex];
                var goalCount = Random.Range(1, MaxCountModifier) * 3;
                var newGoal = _goalSlotFactory.Create(sprite, goalCount);

                var isDuplicate = _goals.Any(existingGoal => existingGoal.Id == newGoal.Id);

                if (isDuplicate)
                {
                    Destroy(newGoal.gameObject);
                    i--;
                }
                else
                {
                    if (_goals.Count < _goalsController.Goals.Count)
                    {
                       _goals.Add(newGoal);
                        _spawnObjectList.Add(newGoal);
                    }
                    else
                    {
                        _spawnObjectList.Add(newGoal);
                    }
                }
            }
            
            _goalsForReplay = _goals.ToList();
            
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
            //_goalsController.ResetGoalsForReplay();
            
            foreach (var item in _collectableItems)
            {
                Destroy(item.gameObject);
            }
            _collectableItems.Clear();
            
            _collectController.ClearSlots();

            SetUpItems();
        }
    }
}