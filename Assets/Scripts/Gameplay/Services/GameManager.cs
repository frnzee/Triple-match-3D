using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Gameplay.Views;
using Gameplay.UI;
using Services;
using Services.Audio;
using Random = UnityEngine.Random;

namespace Gameplay.Services
{
    public class GameManager : MonoBehaviour
    {
        private const int MatchCountMultiplier = 3;
        private const float TargetAspectRatio = 9f / 16f;
        
        [SerializeField] private GameObject _spawnFieldTransform;
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private List<Sprite> _goalIcons;
        [SerializeField] private int _maxCountModifier;
        [SerializeField] private int _itemsToUseCount;
        [SerializeField] private Transform _worldRoot;

        private readonly List<GoalView> _spawnObjectList = new();

        private GoalsController _goalsController;
        private CollectController _collectController;
        private AudioManager _audioManager;
        private SceneNavigation _sceneNavigation;
        
        private GameTimer _gameTimer;
        
        private readonly List<int> _originalCounts = new();
        private readonly List<Sprite> _originalSprites = new();

        private GoalView.Factory _goalViewFactory;
        private CollectableItem.Factory _collectableItemFactory;
        private WinMenu.Factory _winMenuFactory;
        private FailMenu.Factory _failMenuFactory;


        public List<CollectableItem> Items { get; } = new();

        public GameState CurrentGameState { get; private set; }

        [Inject]
        public void Construct(CollectableItem.Factory collectableItemFactory, CollectController collectController,
            GoalsController goalsController, GameTimer gameTimer, WinMenu.Factory winMenuFactory,
            FailMenu.Factory failMenuFactory, GoalView.Factory goalViewFactory, AudioManager audioManager, SceneNavigation sceneNavigation)
        {
            _goalsController = goalsController;
            _collectController = collectController;
            _gameTimer = gameTimer;
            _audioManager = audioManager;
            _sceneNavigation = sceneNavigation;

            _collectableItemFactory = collectableItemFactory;
            _winMenuFactory = winMenuFactory;
            _failMenuFactory = failMenuFactory;
            _goalViewFactory = goalViewFactory;

            _goalsController.Win += OnWin;
            _collectController.Loss += OnLose;
            _gameTimer.TimeIsOver += OnLose;

            CurrentGameState = GameState.None;
            
            SetUpGoals();
        }

        private void Start()
        {
            var currentAspectRatio = (float)Screen.width / Screen.height;

            if (!(Mathf.Abs(currentAspectRatio - TargetAspectRatio) > 0.01f))
            {
                return;
            }
            
            var scaleFactor = new Vector3(TargetAspectRatio / currentAspectRatio, 1f, TargetAspectRatio / currentAspectRatio);
            _worldRoot.localScale = Vector3.Scale(_worldRoot.localScale, scaleFactor);
        }

        private void SetUpGoals()
        {
            var shuffledList = _goalIcons
                .OrderBy(icon => Guid.NewGuid())
                .ToList();

            for (var i = 0; i < _itemsToUseCount; i++)
            {
                var sprite = shuffledList[i];
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
                for (var i = 0; i < element.GoalCount; ++i)
                {
                    var item = _collectableItemFactory.Create(element.Id, _spawnFieldTransform.transform);
                    Items.Add(item);
                }
            }

            CurrentGameState = GameState.Game;
            _gameTimer.ResetTimer();
        }

        private void OnWin()
        {
            _audioManager.PlayWinSound();
            CurrentGameState = GameState.Win;
            _winMenuFactory.Create(_mainCanvas.transform, _gameTimer.GameTime);
            _goalsController.Win -= OnWin;
        }

        private void OnLose()
        {
            _audioManager.PlayLoseSound();
            CurrentGameState = GameState.Lose;
            _failMenuFactory.Create(_mainCanvas.transform);
            _collectController.Loss -= OnLose;
            _gameTimer.TimeIsOver -= OnLose;
        }

        public void RemoveItem(CollectableItem itemToRemove)
        {
            Items.Remove(itemToRemove);
        }

        public void ReplayLevel()
        {
            _goalsController.ClearGoals();

            ClearField();

            ClearOldSpawnObjects();

            _collectController.ClearSlots();
            
            SpawnSameItems();
            SetUpItems();
        }

        private void ClearOldSpawnObjects()
        {
            foreach (var item in _spawnObjectList)
            {
                Destroy(item.gameObject);
            }

            _spawnObjectList.Clear();
        }

        private void SpawnSameItems()
        {
            for (var i = 0; i < _itemsToUseCount; i++)
            {
                var newGoal = _goalViewFactory.Create(_originalSprites[i], _originalCounts[i]);
                _spawnObjectList.Add(newGoal);
            }
        }

        private void ClearField()
        {
            foreach (var item in Items)
            {
                Destroy(item.gameObject);
            }

            Items.Clear();
        }

        public void LoadMainMenu()
        {
            _sceneNavigation.LoadMainMenu();
        }
    }
}