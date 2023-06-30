using Services.Audio;
using UnityEngine;
using Zenject;
using TMPro;

namespace Services
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelButtonText;
        private int _id;
        private AudioManager _audioManager;
        private string _currentLevel;
     
        [Inject]
        public void Construct(int levelNumber, Transform parentTransform, AudioManager audioManager)
        {
            _id = levelNumber;
            _levelButtonText.text = "Level " + levelNumber;
            transform.SetParent(parentTransform);
            _audioManager = audioManager;
        }
        
        public void OnClick()
        {
            _audioManager.Play("ButtonClick");
            SceneNavigation.LoadLevel(_id);
        }

        public class Factory : PlaceholderFactory<int, Transform, LevelItem>
        {
        }
    }
}