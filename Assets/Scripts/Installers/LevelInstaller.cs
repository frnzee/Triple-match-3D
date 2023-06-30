using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _levelItemPrefab;
    
        public override void InstallBindings()
        {
            Container.BindFactory<int, Transform, LevelItem, LevelItem.Factory>()
                .FromComponentInNewPrefab(_levelItemPrefab)
                .AsSingle()
                .Lazy();
        }
    }    
}
