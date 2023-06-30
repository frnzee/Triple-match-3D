using Services.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class ButtonHoverDetector : MonoBehaviour, IPointerEnterHandler
    {
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioManager.Play("ButtonHover");
        }
    }
}