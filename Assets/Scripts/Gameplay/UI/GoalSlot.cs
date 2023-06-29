using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Gameplay.Services;

namespace Gameplay.UI
{
    public class GoalSlot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _goalCountText;
        [SerializeField] private GameObject _goalChild;

        private Vector3 _newPosition;
        private bool _isInitialized;

        public GameObject GoalChild => _goalChild;
        public string Id { get; private set; }
        public int GoalCount { get; private set; }
        public event Action GotCollected;

        [Inject]
        public void Construct(Sprite sprite, Transform goalTransform, int goalCount)
        {
            name = Id = _goalChild.name = _image.name = sprite.name;
            _image.sprite = sprite;
            _newPosition = goalTransform.position;
            GoalCount = goalCount;
            _goalCountText.text = goalCount.ToString();
            InitializePosition();
        }

        public void InitializePosition()
        {
            if (_isInitialized)
            {
                _goalChild.GetComponent<MovingController>().Launch(_newPosition);
                _isInitialized = false;
            }
        }

        public void SetNewGoalChild(GameObject goalChild)
        {
            _isInitialized = true;
            _goalChild = goalChild;
            _image = _goalChild.GetComponent<Image>();
            _goalChild.transform.SetParent(transform);
            _newPosition = transform.position;
            InitializePosition();
        }

        public void GoalDisable()
        {
            _goalChild.SetActive(false);
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

        public class Factory : PlaceholderFactory<Sprite, Transform, int, GoalSlot>
        {
        }
    }
}