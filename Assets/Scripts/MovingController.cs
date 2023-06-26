using System;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    private const float Threshold = 0.01f;

    [SerializeField] private AnimationCurve _curve;

    private readonly float _desiredTime = 1f;
    private Transform _startingPosition;
    private Vector3 _finalPosition;
    private float _elapsedTime;

    public event Action ReachedFinalPosition;
    
    private void Start()
    {
        _startingPosition = transform;
    }

    public void Initialize(Vector3 finalPosition)
    {
        _finalPosition = finalPosition;
    }
    
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        var percentageComplete = _elapsedTime / _desiredTime;
        transform.position = Vector3.Lerp(_startingPosition.position, _finalPosition, _curve.Evaluate(percentageComplete));

        if (!(_elapsedTime >= _desiredTime) && transform.position != _finalPosition)
        {
            return;
        }
        
        _elapsedTime = 0;
        
        ReachedFinalPosition?.Invoke();
    }
}