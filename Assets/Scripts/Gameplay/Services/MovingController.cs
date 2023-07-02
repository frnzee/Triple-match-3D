using System;
using UnityEngine;

namespace Gameplay.Services
{
    public class MovingController : MonoBehaviour
    {
        private const float DesiredTime = 0.5f;

        [SerializeField] private AnimationCurve _curve;

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
    
            if (MoveByCurve())
            {
                return;
            }
    
            ReachedFinalPosition?.Invoke();
        }
    
        private bool MoveByCurve()
        {
            _elapsedTime += Time.deltaTime;
    
            var percentageComplete = _elapsedTime / DesiredTime;
            transform.position =
                Vector3.Lerp(_startingPosition.position, _finalPosition, _curve.Evaluate(percentageComplete));
    
            if (!(_elapsedTime >= DesiredTime) && transform.position != _finalPosition)
            {
                return true;
            }
    
            ResetMovementState();
            return false;
        }

        private void ResetMovementState()
        {
            _elapsedTime = 0;
            _canMove = false;
        }
    }
}