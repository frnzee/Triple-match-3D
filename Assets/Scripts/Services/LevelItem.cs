using UnityEngine;
using Zenject;
using TMPro;

namespace Services
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelButtonText;
        
        private int _id;
        private string _currentLevel;
     
        [Inject]
        public void Construct(int levelNumber, Transform parentTransform)
        {
            _id = levelNumber;
            _levelButtonText.text = "Level " + levelNumber;
            transform.SetParent(parentTransform);
            transform.localScale = new Vector3(1f, 1f, 1f);
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