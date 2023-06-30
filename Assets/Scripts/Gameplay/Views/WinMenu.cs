using UnityEngine;
using TMPro;
using Zenject;
using Services;
using Services.Audio;

namespace Gameplay.Views
{
    public class WinMenu : MonoBehaviour
    {
        private const string ContinueButtonText = "Continue";
        private const float TimeDivider = 60f;
        
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private GameObject[] _stars;

        private SceneNavigation _sceneNavigation;
        private AudioManager _audioManager;
        private int _starsCount;

        [Inject]
        public void Construct(Transform parentTransform, float timeSpent, int starsCount, SceneNavigation sceneNavigation, AudioManager audioManager)
        {
            _sceneNavigation = sceneNavigation;
            _audioManager = audioManager;

            var minutes = Mathf.FloorToInt(timeSpent / TimeDivider);
            var seconds = Mathf.FloorToInt(timeSpent % TimeDivider);
            
            _timeText.text = $"{minutes:0}:{seconds:00}";
            _buttonText.text = ContinueButtonText;
            _starsCount = starsCount;
            transform.SetParent(parentTransform, false);
        }
        
        public void LoadMainMenu()
        {
            _audioManager.Play("ButtonClick");
            _sceneNavigation.LoadMainMenu();
        }
        
        public class Factory : PlaceholderFactory<Transform, float, int, WinMenu>
        {
        }
    }   
}