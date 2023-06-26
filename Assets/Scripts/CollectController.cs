using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectController : MonoBehaviour, ICollectController
{
    [SerializeField] private List<GameObject> _itemsBases;

    private List<CollectedItem> _items;
    private CollectedItem.Factory _collectedItemsFactory;
    private GoalsController _goalsController;

    private int _currentIndex;

    [Inject]
    public void Construct(CollectedItem.Factory collectedItemFactory, GoalsController goalsController)
    {
        _collectedItemsFactory = collectedItemFactory;
        _goalsController = goalsController;
        _items = new List<CollectedItem>();
    }

    public void CollectItem(Vector3 itemPosition, Sprite itemSprite)
    {
        var availableIndex = FindAvailableSpot(itemSprite);

        var collectedItem = _collectedItemsFactory.Create(itemSprite.name);
        collectedItem.transform.position = itemPosition;
        collectedItem.transform.SetParent(_itemsBases[availableIndex].transform);
        collectedItem.GetComponent<Image>().sprite = itemSprite;
        collectedItem.GetComponent<MovingController>().Initialize(_itemsBases[availableIndex].transform.position); 
        _goalsController.CheckGoalMatch(itemSprite.name);

        _items.Insert(availableIndex, collectedItem);

        var matchIndex = CheckForMatches(itemSprite);
        if (matchIndex >= 0)
        {
            _currentIndex = matchIndex;
            collectedItem.GetComponent<MovingController>().ReachedFinalPosition += OnReachedFinalPosition;
        }
    }

    private int FindAvailableSpot(Object sprite)
    {
        if (_items.Count != 0)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].transform.parent == null)
                {
                    if (i >= 2 && i < _items.Count && _items[i - 2].name == sprite.name &&
                        _items[i - 1].name == sprite.name && _items[i].name == sprite.name)
                    {
                        return i;
                    }

                    if (i >= 1 && i < _items.Count && _items[i - 1].name == sprite.name)
                    {
                        return i;
                    }

                    if (i >= 0 && i < _items.Count && _items[i].name != sprite.name)
                    {
                        return i;
                    }
                }

                if (i < _items.Count - 1 && _items[i].name == sprite.name && _items[i + 1].name == sprite.name)
                {
                    ShiftItems(i + 1);
                    return i + 2;
                }

                if (_items[i].name == sprite.name)
                {
                    ShiftItems(i);
                    return i + 1;
                }
            }
        }

        return _items.Count;
    }

    private int CheckForMatches(Object sprite)
    {
        for (int i = 0; i < _items.Count - 2; i++)
        {
            if (_items[i].name == sprite.name && _items[i + 1].name == sprite.name && _items[i + 2].name == sprite.name)
            {
                return i;
            }
        }

        return -1;
    }

    private void RemoveAndShiftItems()
    {
        for (int i = _currentIndex + 2; i >= _currentIndex; --i)
        {
            Destroy(_items[i].gameObject);
            _items.RemoveAt(i);
        }

        for (int i = 0; i < _items.Count; ++i)
        {
            var item = _items[i];
            var parentTransform = _itemsBases[i].transform;
            item.transform.SetParent(parentTransform);
            item.GetComponent<MovingController>().Initialize(parentTransform.position);
        }
    }

    private void ShiftItems(int index)
    {
        if (_items.Count < 7)
        {
            for (int i = _items.Count - 1; i > index; --i)
            {
                var item = _items[i];
                var targetIndex = i + 1;
                var parentTransform = _itemsBases[targetIndex].transform;
                item.transform.SetParent(parentTransform);
                item.GetComponent<MovingController>().Initialize(parentTransform.position);
            }
        }
    }

    private void OnReachedFinalPosition()
    {
        RemoveAndShiftItems();
    }
}