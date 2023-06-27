using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _spawnFieldTransform;

    private ItemController.Factory _itemControllerFactory;
    private List<GoalSlot> _goalSlots;

    [Inject]
    public void Construct(ItemController.Factory itemControllerFactory, CollectController collectController, GoalsController goalsController)
    {
        _itemControllerFactory = itemControllerFactory;
    }

    public void SetUpItems(List<GoalSlot> goalSlots)
    {
        _goalSlots = goalSlots;
        foreach (var goal in _goalSlots)
        {
            for (int i = 0; i < goal.GoalCount; ++i)
            {
                _itemControllerFactory.Create(goal.name, _spawnFieldTransform.transform);
            }
        }
    }
}