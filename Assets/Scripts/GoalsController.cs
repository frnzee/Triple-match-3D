using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GoalsController : MonoBehaviour
{
    private const int MaxCountModifier = 3;

    [SerializeField] private List<Sprite> _sprites;

    private readonly List<GoalSlot> _goals = new();
    private readonly List<GoalSlot> _spawnObjectsList = new();

    private GameManager _gameManager;
    private GoalSlot.Factory _goalSlotFactory;

    private int _currentindex;
    private GoalSlot _goal;

    [Inject]
    public void Construct(GameManager gameManager, GoalSlot.Factory goalSlotFactory)
    {
        _gameManager = gameManager;
        _goalSlotFactory = goalSlotFactory;
    }

    private void Start()
    {
        for (int i = 0; i < _sprites.Count; ++i)
        {
            var randomIndex = Random.Range(0, _sprites.Count);
            var sprite = _sprites[randomIndex];

            var newGoal = _goalSlotFactory.Create(sprite, transform, MaxCountModifier);

            var isDuplicate = _goals.Any(existingGoal => existingGoal.name == newGoal.name);
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
    }

    public void CheckGoalMatch(string itemName)
    {
        for (var i = 0; i < _goals.Count; i++)
        {
            _goal = _goals[i];
            if (_goal.name == itemName)
            {
                _goal.DecreaseGoalCount();
                _currentindex = i;
                if (_goal.GoalCount == 0)
                {
                    _goal.GotCollected -= OnGotCollected;
                    break;
                }
            }
        }
    }

    private void RemoveAndShiftGoals()
    {
        _goals[_currentindex].GoalDisable();

        var goalToMove = _goals[_currentindex];
        _goals.RemoveAt(_currentindex);
        _goals.Add(goalToMove);

        for (int i = _currentindex; i < _goals.Count; ++i)
        {
            _goals[i].SetNewParent(_goals[i].transform);
        }
    }

    private void OnGotCollected()
    {
        RemoveAndShiftGoals();
    }
}