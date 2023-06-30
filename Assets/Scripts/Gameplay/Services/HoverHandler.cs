using Services.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Services
{
    public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float DefaultScale = 1f;
        private const float HoverScale = 1.1f;
        private const float HoverDuration = 0.2f;
        
        private Vector3 _originalScale;
        private float _hoverTimer;
        private bool _isHovering;

        private AudioManager _audioManager;

        [Inject]
        public void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        private void Start()
        {
            _originalScale = transform.localScale;
        }

        private void Update()
        {
            if (_isHovering)
            {
                HoverSize();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioManager.Play("ItemHover");
            _isHovering = true;
            HoverSize();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetHoverSize();
        }

        private void HoverSize()
        {
            _hoverTimer += Time.deltaTime;
            var time = Mathf.Clamp01(_hoverTimer / HoverDuration);
            var scale = Mathf.Lerp(DefaultScale, HoverScale, time);
            transform.localScale = _originalScale * scale;

            if (_hoverTimer >= HoverDuration)
            {
                _isHovering = false;
            }
        }

        private void ResetHoverSize()
        {
            _isHovering = false;
            _hoverTimer = 0f;
            transform.localScale = _originalScale;
        }
    }
}