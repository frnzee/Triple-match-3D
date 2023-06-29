using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;
using Gameplay.UI;

namespace Gameplay.Services
{
    public class GoalsController : MonoBehaviour
    {
        private const int MaxCountModifier = 1;

        [SerializeField] private List<Sprite> _goalIcons;

        private List<GoalSlot> _goals = new();
        private readonly List<GoalSlot> _spawnObjectList = new();
        private List<GoalSlot> _goalsForReplay;

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
                var goalCount = Random.Range(1, MaxCountModifier) * 3;
                var newGoal = _goalSlotFactory.Create(sprite, transform, goalCount);

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
                        _spawnObjectList.Add(newGoal);
                    }
                    else
                    {
                        _spawnObjectList.Add(newGoal);
                    }
                }
            }

            _goalsForReplay = _goals.ToList();

            _gameManager.SetUpItems(_spawnObjectList);

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

            _goals[^1].SetNewGoalChild(_goal.GoalChild);
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

        public void ResetGoalsForReplay()
        {
            foreach (var oldGoal in _goals)
            {
                Destroy(oldGoal.gameObject);
            }

            _goals.Clear();

            Debug.Log(_goalsForReplay.Count);
            for (int i = 0; i < _goalsForReplay.Count; i++)
            {
                Debug.Log("Restore goals " + i);
                var newGoal = _goalSlotFactory.Create(_goalsForReplay[i].GetComponentInChildren<Image>().sprite, transform,
                    _goalsForReplay[i].GoalCount);

                newGoal.transform.SetParent(transform);
                _goals.Add(newGoal);
                _spawnObjectList.Add(newGoal);
            }
        }
    }
}