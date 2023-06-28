using UnityEngine;
using Services.Loading;
using Zenject;

namespace Installers
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScenes();
        }

        private void BindScenes()
        {
            Container.Bind<SceneNames>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}