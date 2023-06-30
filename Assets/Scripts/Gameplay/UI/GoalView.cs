using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Gameplay.Services;

namespace Gameplay.UI
{
    public class GoalView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _goalCountText;

        private int _currentGoalCount;
        private GoalsController _goalsController;
        
        public string Id { get; private set; }
        public int GoalCount { get; private set; }
        public event Action GotCollected;

        [Inject]
        public void Construct(Sprite sprite, int goalCount, GoalsController goalsController)
        {
            _goalsController = goalsController;
            Id = _image.name = sprite.name;
            _image.sprite = sprite;
            GoalCount = goalCount;
            _goalCountText.text = goalCount.ToString();
        }
        
        private void Start()
        {
            _goalsController.AddGoal(this);
        }
        
        public void InitializePosition()
        {
            GetComponent<MovingController>().Launch(transform.parent.position);
        }

        public void DecreaseGoalCount()
        {
            ApplyGoal();
        }

        private void ApplyGoal()
        {
            GoalCount--;
            _goalCountText.text = GoalCount.ToString();

            if (GoalCount <= 0)
            {
                GotCollected?.Invoke();
            }
        }

        public class Factory : PlaceholderFactory<Sprite, int, GoalView>
        {
        }
    }
}