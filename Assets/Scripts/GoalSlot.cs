using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using Zenject;

public class GoalSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _goalCountText;
    [SerializeField] private GameObject _goalTransform;

    private int _goalCount;

    public int GoalCount
    {
        get => _goalCount;
        private set
        {
            _goalCount = value;
            if (_goalCount <= 0)
            {
                GotCollected.Invoke();
            }
        }
    }

    private Vector3 _newPosition;

    public event Action GotCollected;

    [Inject]
    public void Construct(Sprite sprite, Transform goalTransform, int maxCountModifier)
    {
        name = _goalTransform.name = _image.name = sprite.name;
        _image.sprite = sprite;
        _newPosition = goalTransform.position;
        _goalCount = Random.Range(1, maxCountModifier) * 3;
        _goalCountText.text = _goalCount.ToString();
        InitializePosition();
    }

    public void InitializePosition()
    {
        _newPosition = _goalTransform.transform.parent.position;
        _goalTransform.GetComponent<MovingController>().Initialize(_newPosition);
    }

    public void SetNewParent(Transform newTransform)
    {
        _goalTransform.transform.SetParent(newTransform);
        InitializePosition();
    }

    public void GoalDisable()
    {
        _goalTransform.SetActive(false);
    }

    public void DecreaseGoalCount()
    {
        --GoalCount;
        _goalCountText.text = _goalCount.ToString();
    }

    private void LateUpdate()
    {
        _goalTransform.GetComponent<MovingController>().Initialize(_goalTransform.transform.parent.position);
    }

    public class Factory : PlaceholderFactory<Sprite, Transform, int, GoalSlot>
    {
    }
}