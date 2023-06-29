using UnityEngine;
using Zenject;
using Gameplay;
using Gameplay.Views;
using Gameplay.Services;
using Gameplay.UI;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Instances")] [SerializeField] private GameManager _gameManager;
        [SerializeField] private CollectController _collectController;
        [SerializeField] private GoalsController _goalsController;
        [SerializeField] private GameTimer _gameTimer;

        [Header("Prefabs")] [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _collectedItemPrefab;
        [SerializeField] private GameObject _goalSlotPrefab;
        [SerializeField] private GameObject _winMenuPrefab;
        [SerializeField] private GameObject _failMenuPrefab;

        public override void InstallBindings()
        {
            BindFactories();
            BindControllers();
        }

        private void BindControllers()
        {
            Container.Bind<GameManager>()
                .FromInstance(_gameManager)
                .AsSingle()
                .NonLazy();

            Container.Bind<CollectController>()
                .FromInstance(_collectController)
                .AsSingle()
                .NonLazy();

            Container.Bind<GoalsController>()
                .FromInstance(_goalsController)
                .AsSingle()
                .NonLazy();

            Container.Bind<GoalSlot>()
                .FromComponentInNewPrefab(_goalSlotPrefab)
                .AsCached()
                .Lazy();

            Container.Bind<GameTimer>()
                .FromInstance(_gameTimer)
                .AsSingle()
                .NonLazy();
        }

        private void BindFactories()
        {
            Container.BindFactory<string, Transform, CollectableItem, CollectableItem.Factory>()
                .FromComponentInNewPrefab(_itemPrefab)
                .AsSingle()
                .Lazy();

            Container.BindFactory<Sprite, Transform, int, GoalSlot, GoalSlot.Factory>()
                .FromComponentInNewPrefab(_goalSlotPrefab)
                .UnderTransformGroup("GoalSlot")
                .AsSingle()
                .Lazy();

            Container.BindFactory<Vector3, Transform, Sprite, CollectedItem, CollectedItem.Factory>()
                .FromComponentInNewPrefab(_collectedItemPrefab)
                .AsSingle()
                .Lazy();

            Container.BindFactory<Transform, float, int, WinMenu, WinMenu.Factory>()
                .FromComponentInNewPrefab(_winMenuPrefab)
                .AsSingle()
                .Lazy();

            Container.BindFactory<Transform, FailMenu, FailMenu.Factory>()
                .FromComponentInNewPrefab(_failMenuPrefab)
                .AsSingle()
                .Lazy();
        }
    }
}