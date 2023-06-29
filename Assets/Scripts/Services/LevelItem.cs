using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using TMPro;

namespace Services
{
    public class LevelItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI _levelButtonText;
        private LevelSelector _levelSelector;
        private string _currentLevel;
     
        [Inject]
        public void Construct(int levelNumber, Transform parentTransform, LevelSelector levelSelector)
        {
            _levelSelector = levelSelector;
            name = "Level" + levelNumber;
            _levelButtonText.text = "Level " + levelNumber;
            transform.SetParent(parentTransform);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _levelSelector.LoadLevel(name);
        }

        public class Factory : PlaceholderFactory<int, Transform, LevelItem>
        {
        }
    }
}