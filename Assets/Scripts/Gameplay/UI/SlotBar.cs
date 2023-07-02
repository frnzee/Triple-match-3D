using System.Collections.Generic;
using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class SlotBar : MonoBehaviour
    {
        [SerializeField] private List<Transform> _goalSlots;

        private GoalsController _goalsController;
        public int SlotCount => _goalSlots.Count;

        [Inject]
        public void Construct(GoalsController goalsController)
        {
            _goalsController = goalsController;
        }

        public void PlaceGoalsToSlots()
        {
            for (var i = 0; i < _goalsController.Goals.Count - 1; i++)
            {
                var goal = _goalsController.Goals[i];
                
                if (goal.transform.parent == _goalSlots[i].transform)
                {
                    continue;
                }
                
                goal.transform.SetParent(_goalSlots[i].transform, false);
                var rectTransform = goal.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}