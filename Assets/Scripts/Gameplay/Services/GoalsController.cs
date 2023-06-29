using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Gameplay.UI;

namespace Gameplay.Services
{
    public class GoalsController : MonoBehaviour
    {
        private SlotBar _slotBar;

        private int _currentIndex;
        private int _goalsToWin;

        public List<GoalView> Goals { get; } = new();

        public event Action GotWin;

        [Inject]
        public void Construct(SlotBar slotBar)
        {
            _slotBar = slotBar;
        }

        public void AddGoal(GoalView newGoal)
        {
            if (Goals.Count() <= _slotBar.SlotCount)
            {
                Goals.Add(newGoal);
                newGoal.GotCollected += OnGotCollected;
                return;
            }
            
            _slotBar.PlaceGoalsToSlots();
            _goalsToWin = Goals.Count();
        }

        public void CheckGoalMatch(string itemName)
        {
            for (var i = 0; i < Goals.Count() - 1; i++)
            {
                var goal = Goals[i].GetComponentInChildren<GoalView>();
                if (goal.Id == itemName)
                {
                    goal.DecreaseGoalCount();
                    _currentIndex = i;
                    break;
                }
            }
        }

        private void OnGotCollected()
        {
            Goals[_currentIndex].GetComponentInChildren<GoalView>().GotCollected -= OnGotCollected;
            Destroy(Goals[_currentIndex].gameObject);
            Goals.RemoveAt(_currentIndex);
            _slotBar.PlaceGoalsToSlots();
            _goalsToWin--;

            if (_goalsToWin <= 1)
            {
                GotWin?.Invoke();
            }
        }
    }
}