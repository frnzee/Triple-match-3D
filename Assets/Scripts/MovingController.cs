using System;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;

    private readonly float _desiredTime = 1f;
    private Transform _startingPosition;
    private Vector3 _finalPosition;
    private float _elapsedTime;
    private bool _canMove;
    
    public event Action ReachedFinalPosition;

    public void Launch(Vector3 finalPosition)
    {
        _canMove = true;
        _startingPosition = transform;
        _finalPosition = finalPosition;
    }
    
    private void Update()
    {
        if (!_canMove)
        {
            return;
        }
        
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