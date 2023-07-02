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

        private SceneNavigation _sceneNavigation;

        [Inject]
        public void Construct(Transform parentTransform, float timeSpent, SceneNavigation sceneNavigation)
        {
            _sceneNavigation = sceneNavigation;

            var minutes = Mathf.FloorToInt(timeSpent / TimeDivider);
            var seconds = Mathf.FloorToInt(timeSpent % TimeDivider);
            
            _timeText.text = $"{minutes:0}:{seconds:00}";
            _buttonText.text = ContinueButtonText;
            transform.SetParent(parentTransform, false);
        }
        
        public void LoadMainMenu()
        {
            _sceneNavigation.LoadMainMenu();
        }
        
        public class Factory : PlaceholderFactory<Transform, float, WinMenu>
        {
        }
    }   
}