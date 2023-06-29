using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _levelItemPrefab;
        [SerializeField] private LevelSelector _levelSelector;
    
        public override void InstallBindings()
        {
            Container.Bind<LevelSelector>()
                .FromInstance(_levelSelector)
                .AsSingle()
                .Lazy();
            
            Container.BindFactory<int, Transform, LevelItem, LevelItem.Factory>()
                .FromComponentInNewPrefab(_levelItemPrefab)
                .AsSingle()
                .Lazy();
        }
    }    
}
