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
            for (int i = 0; i < _goalsController.Goals.Count - 1; i++)
            {
                var goal = _goalsController.Goals[i];
                goal.transform.SetParent(_goalSlots[i].transform, false);
                //goal.Init(_goalSlots[i].transform);
                //goal.transform.localPosition = Vector3.zero;
                goal.InitializePosition();
            }
        }
    }
}