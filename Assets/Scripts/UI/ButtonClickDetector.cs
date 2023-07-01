using Services.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class ButtonClickDetector : MonoBehaviour, IPointerUpHandler
    {
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _audioManager.PlayButtonClickSound();
        }
    }
}