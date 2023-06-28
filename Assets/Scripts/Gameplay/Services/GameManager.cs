using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Gameplay.Views;
using Gameplay.UI;

namespace Gameplay.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnFieldTransform;
        [SerializeField] private GameObject _mainCanvas;
    
        private CollectableItem.Factory _itemControllerFactory;
        private GoalsController _goalsController;
        private CollectController _collectController;
        private WinMenu.Factory _winMenuFactory;
        private FailMenu.Factory _failMenuFactory;
        private List<GoalSlot> _goalSlots;
    
        [Inject]
        public void Construct(CollectableItem.Factory itemControllerFactory, CollectController collectController,
            GoalsController goalsController, WinMenu.Factory winMenuFactory, FailMenu.Factory failMenuFactory)
        {
            _itemControllerFactory = itemControllerFactory;
            _goalsController = goalsController;
            _collectController = collectController;
            _winMenuFactory = winMenuFactory;
            _failMenuFactory = failMenuFactory;
    
            _goalsController.GotWin += OnWin;
            _collectController.GotLoss += OnLose;
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
    
        private void OnWin()
        {
            _winMenuFactory.Create(_mainCanvas.transform, 65f, 2);
        }
    
        private void OnLose()
        {
            _failMenuFactory.Create(_mainCanvas.transform);
        }
    }
}