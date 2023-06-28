using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Gameplay.UI;

namespace Gameplay.Services
{
    public class GoalsController : MonoBehaviour
    {
        private const int MaxCountModifier = 1;

        [SerializeField] private List<Sprite> _goalIcons;

        private readonly List<GoalSlot> _goals = new();
        private readonly List<GoalSlot> _spawnObjectsList = new();

        private GameManager _gameManager;
        private GoalSlot.Factory _goalSlotFactory;

        private int _currentIndex;
        private int _goalsForTheWin;
        private GoalSlot _goal;

        public event Action GotWin;

        [Inject]
        public void Construct(GameManager gameManager, GoalSlot.Factory goalSlotFactory)
        {
            _gameManager = gameManager;
            _goalSlotFactory = goalSlotFactory;
        }

        private void Start()
        {
            for (int i = 0; i < _goalIcons.Count; ++i)
            {
                var randomIndex = Random.Range(0, _goalIcons.Count);
                var sprite = _goalIcons[randomIndex];

                var newGoal = _goalSlotFactory.Create(sprite, transform, MaxCountModifier);

                var isDuplicate = _goals.Any(existingGoal => existingGoal.Id == newGoal.Id);
                if (isDuplicate)
                {
                    Destroy(newGoal.gameObject);
                    i--;
                }
                else
                {
                    if (_goals.Count < 4)
                    {
                        newGoal.transform.SetParent(transform);
                        _goals.Add(newGoal);
                        _spawnObjectsList.Add(newGoal);
                    }
                    else
                    {
                        _spawnObjectsList.Add(newGoal);
                    }
                }
            }

            _gameManager.SetUpItems(_spawnObjectsList);

            foreach (var goal in _goals)
            {
                goal.InitializePosition();
                goal.GotCollected += OnGotCollected;
            }

            _goalsForTheWin = _goals.Count;
        }

        public void CheckGoalMatch(string itemName)
        {
            for (var i = 0; i < _goals.Count; i++)
            {
                var goal = _goals[i];
                if (goal.GoalChild.name == itemName)
                {
                    goal.DecreaseGoalCount();
                    _currentIndex = i;
                    if (goal.GoalCount <= 0)
                    {
                        goal.GotCollected -= OnGotCollected;
                        break;
                    }
                }
            }
        }

        private void RemoveAndShiftGoals()
        {
            _goal = _goals[_currentIndex];
            _goal.GoalDisable();

            for (int i = _currentIndex; i < _goals.Count - 1; i++)
            {
                _goals[i].SetNewGoalChild(_goals[i + 1].GoalChild);
            }

            _goals[_goals.Count - 1].SetNewGoalChild(_goal.GoalChild);
            
            foreach (var goal in _goals)
            {
                Debug.Log(goal);
                Debug.Log(goal.GoalChild);
            }
            Debug.Log("===============================");
        }

        private void OnGotCollected()
        {
            RemoveAndShiftGoals();

            _goalsForTheWin--;

            if (_goalsForTheWin <= 0)
            {
                GotWin?.Invoke();
            }
        }
    }
}