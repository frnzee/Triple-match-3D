using Services.Audio;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoostersBase : MonoBehaviour
    {
        private Fan.Factory _fanFactory;
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(Fan.Factory fanFactory, AudioManager audioManager)
        {
            _fanFactory = fanFactory;
            _audioManager = audioManager;
        }
        public void UseFan()
        {
            _audioManager.Play("ButtonClick");
            _audioManager.Play("Fan");
            _fanFactory.Create();
        }
    }
}
