using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Gameplay.UI;
using Services.Audio;
using TMPro;

namespace Gameplay.Services
{
    public class CollectController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _itemsBases;

        private readonly List<CollectedItem> _items = new();
        private CollectedItem.Factory _collectedItemsFactory;
        private GoalsController _goalsController;
        private AudioManager _audioManager;

        private int _currentIndex;

        public event Action GotLoss;

        [Inject]
        public void Construct(CollectedItem.Factory collectedItemFactory, GoalsController goalsController, AudioManager audioManager)
        {
            _collectedItemsFactory = collectedItemFactory;
            _goalsController = goalsController;
            _audioManager = audioManager;
        }

        public void CollectItem(Vector3 itemPosition, Sprite itemIcon)
        {
            if (_items.Count < _itemsBases.Count)
            {
                var availableIndex = FindAvailableSpot(itemIcon);

                var collectedItem = _collectedItemsFactory.Create(itemPosition, _itemsBases[availableIndex].transform, itemIcon);

                _goalsController.CheckGoalMatch(itemIcon.name);

                _items.Insert(availableIndex, collectedItem);

                var matchIndex = CheckForMatches(itemIcon);
                if (matchIndex >= 0)
                {
                    _currentIndex = matchIndex;
                    collectedItem.GetComponent<MovingController>().ReachedFinalPosition += OnReachedFinalPosition;
                }
                else
                {
                    collectedItem.GetComponent<MovingController>().ReachedFinalPosition += OnReachedFinalPositionCheckForLoss;
                }
            }
        }

        private void OnReachedFinalPositionCheckForLoss()
        {
            if (_items.Count == _itemsBases.Count)
            {
                GotLoss?.Invoke();
            }
        }

        private int FindAvailableSpot(Sprite sprite)
        {
            if (_items.Count != 0)
            {
                int itemCount = _items.Count;

                for (int i = 0; i < itemCount; i++)
                {
                    var currentItem = _items[i];

                    if (currentItem.transform.parent == null)
                    {
                        if (i >= 1 && _items[i - 1].Id == sprite.name)
                        {
                            return i;
                        }

                        if (currentItem.Id != sprite.name)
                        {
                            return i;
                        }
                    }

                    if (i < itemCount - 1 && currentItem.Id == sprite.name && _items[i + 1].Id == sprite.name)
                    {
                        ShiftItems(i + 1);
                        return i + 2;
                    }

                    if (currentItem.Id == sprite.name)
                    {
                        ShiftItems(i);
                        return i + 1;
                    }
                }
            }

            return _items.Count;
        }

        private int CheckForMatches(Sprite sprite)
        {
            for (int i = 0; i < _items.Count - 2; i++)
            {
                if (_items[i].Id == sprite.name && _items[i + 1].Id == sprite.name && _items[i + 2].Id == sprite.name)
                {
                    return i;
                }
            }

            return -1;
        }

        private void RemoveAndShiftItems()
        {
            _audioManager.Play("3ItemsMatch");
            int itemCount = _items.Count;

            if (_currentIndex >= 0 && _currentIndex + 2 < itemCount)
            {
                for (int i = _currentIndex + 2; i >= _currentIndex; --i)
                {
                    Destroy(_items[i].gameObject);
                    _items.RemoveAt(i);
                }
            }

            for (int i = 0; i < _items.Count; ++i)
            {
                var item = _items[i];
                var parentTransform = _itemsBases[i].transform;
                item.transform.SetParent(parentTransform);
                item.GetComponent<MovingController>().Launch(parentTransform.position);
            }
        }

        private void ShiftItems(int index)
        {
            if (_items.Count < _itemsBases.Count)
            {
                for (int i = _items.Count - 1; i > index; --i)
                {
                    var item = _items[i];
                    var targetIndex = i + 1;
                    var parentTransform = _itemsBases[targetIndex].transform;
                    item.transform.SetParent(parentTransform);
                    item.GetComponent<MovingController>().Launch(parentTransform.position);
                }
            }
        }

        public void ClearSlots()
        {
            foreach (var oldItem in _items)
            {
                Destroy(oldItem.gameObject);
            }

            _items.Clear();
        }

        private void OnReachedFinalPosition()
        {
            RemoveAndShiftItems();
        }
    }
}