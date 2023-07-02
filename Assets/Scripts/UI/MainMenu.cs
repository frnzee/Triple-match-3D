using Services;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private const int LevelsCount = 5;

        [SerializeField] private Transform _parentTransform;

        private LevelItem.Factory _levelItemFactory;

        [Inject]
        public void Construct(LevelItem.Factory levelItemFactory)
        {
            _levelItemFactory = levelItemFactory;
        }
        
        private void Start()
        {
            for (var i = 1; i <= LevelsCount; i++)
            {
                _levelItemFactory.Create(i, _parentTransform);
            }
        }
    }
}
