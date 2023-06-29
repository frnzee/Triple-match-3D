using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

namespace Services
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransform;
        
        private SceneNames _sceneNames;
        private LevelItem.Factory _levelItemFactory;

        [Inject]
        public void Construct(LevelItem.Factory levelItemFactory, SceneNames sceneNames)
        {
            _levelItemFactory = levelItemFactory;
            _sceneNames = sceneNames;
        }

        public void Start()
        {
            for (int i = 1; i <= 5; i++)
            {
                _levelItemFactory.Create(i, _parentTransform);
            }
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}