using Services;
using Zenject;

namespace Installers
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScenes();

            Container.Bind<SceneNavigation>()
                .FromNew()
                .AsSingle()
                .Lazy();
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