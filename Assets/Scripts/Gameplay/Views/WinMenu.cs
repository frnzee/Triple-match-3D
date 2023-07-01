using UnityEngine;
using TMPro;
using Zenject;
using Services;

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
        private int _starsCount;

        [Inject]
        public void Construct(Transform parentTransform, float timeSpent, int starsCount, SceneNavigation sceneNavigation)
        {
            _sceneNavigation = sceneNavigation;

            var minutes = Mathf.FloorToInt(timeSpent / TimeDivider);
            var seconds = Mathf.FloorToInt(timeSpent % TimeDivider);
            
            _timeText.text = $"{minutes:0}:{seconds:00}";
            _buttonText.text = ContinueButtonText;
            _starsCount = starsCount;
            transform.SetParent(parentTransform, false);
        }
        
        public void LoadMainMenu()
        {
            SceneNavigation.LoadMainMenu();
        }
        
        public class Factory : PlaceholderFactory<Transform, float, int, WinMenu>
        {
        }
    }   
}