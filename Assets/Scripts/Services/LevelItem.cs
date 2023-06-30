using UnityEngine;
using Zenject;
using TMPro;

namespace Services
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelButtonText;
        private int _id;
        private SceneNavigation _sceneNavigation;
        private string _currentLevel;
     
        [Inject]
        public void Construct(int levelNumber, Transform parentTransform, SceneNavigation sceneNavigation)
        {
            _sceneNavigation = sceneNavigation;
            _id = levelNumber;
            _levelButtonText.text = "Level " + levelNumber;
            transform.SetParent(parentTransform);
        }
        
        public void OnClick()
        {
            SceneNavigation.LoadLevel(_id);
        }

        public class Factory : PlaceholderFactory<int, Transform, LevelItem>
        {
        }
    }
}