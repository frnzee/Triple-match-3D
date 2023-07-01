using Gameplay.Services;
using Services.Audio;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class Fan : MonoBehaviour
    {
        private const float Lifetime = 1f;
        
        [SerializeField] private float _fanForce = 10f;
        
        private GameManager _gameManager;
        private AudioManager _audioManager;

        [Inject]
        public void Construct(GameManager gameManager, AudioManager audioManager)
        {
            _gameManager = gameManager;
            _audioManager = audioManager;
        }

        private void Start()
        {
            _audioManager.PlayFanSound();
            
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