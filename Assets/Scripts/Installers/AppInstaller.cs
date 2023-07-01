using Services;
using UnityEngine;
using Zenject;
using Services.Audio;

namespace Installers
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _audioManagerPrefab;

        public override void InstallBindings()
        {
            BinsSceneNavigation();
            BindAudio();
        }

        private void BinsSceneNavigation()
        {
            Container.Bind<SceneNavigation>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindAudio()
        {
            Container.Bind<AudioManager>()
                .FromComponentInNewPrefab(_audioManagerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}