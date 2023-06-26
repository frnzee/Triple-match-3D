using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
                SpawnObjects(goal.name);
            }
        }
    }

    private void SpawnObjects(string itemName)
    {
        var itemPosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0.5f, 1.5f), Random.Range(-2.5f, 2.5f));
        var itemRotation = Random.rotation;

        var spawnedObject = _itemControllerFactory.Create(itemName);
        var spawnedObjectTransform = spawnedObject.transform;
        spawnedObjectTransform.position = itemPosition;
        spawnedObjectTransform.rotation = itemRotation;
        spawnedObject.transform.SetParent(_spawnFieldTransform.transform);
    }
}