using Gameplay.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class Fan : MonoBehaviour
    {
        private const float Lifetime = 1f;
        [SerializeField] private float _fanForce = 10f;
        
        private GameManager _gameManager;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
            
            foreach (var item in _gameManager.Items)
            {
                var otherRigidbody = item.GetComponent<Rigidbody>();
                var fanDirection = Vector3.up;
                otherRigidbody.AddForce(fanDirection * _fanForce, ForceMode.Force);
            }
            
            Destroy(gameObject, Lifetime);
        }

        public class Factory : PlaceholderFactory<Fan>
        {
        }
    }
}