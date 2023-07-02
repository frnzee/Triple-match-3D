using UnityEngine;
using Zenject;
using Gameplay;
using Gameplay.Boosters;
using Gameplay.Views;
using Gameplay.Services;
using Gameplay.UI;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Instances")] 
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private CollectController _collectController;
        [SerializeField] private GoalsController _goalsController;
        [SerializeField] private GameTimer _gameTimer;
        [SerializeField] private SlotBar _slotBar;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _collectedItemPrefab;
        [SerializeField] private GameObject _goalSlotPrefab;

        [Header("Menus")]
        [SerializeField] private GameObject _winMenuPrefab;
        [SerializeField] private GameObject _failMenuPrefab;

        [Header("Boosters")]
        [SerializeField] private GameObject _fanPrefab;
        public override void InstallBindings()
        {
            BindFactories();
            BindControllers();
            BindMenus();
            BindBoosters();
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

            Container.Bind<GoalView>()
                .FromComponentInNewPrefab(_goalSlotPrefab)
                .AsCached()
                .Lazy();

            Container.Bind<GameTimer>()
                .FromInstance(_gameTimer)
                .AsSingle()
                .NonLazy();

            Container.Bind<SlotBar>()
                .FromInstance(_slotBar)
                .AsSingle()
                .NonLazy();
        }

        private void BindFactories()
        {
            Container.BindFactory<string, Transform, CollectableItem, CollectableItem.Factory>()
                .FromComponentInNewPrefab(_itemPrefab)
                .AsSingle()
                .Lazy();

            Container.BindFactory<Sprite, int, GoalView, GoalView.Factory>()
                .FromComponentInNewPrefab(_goalSlotPrefab)
                .UnderTransformGroup("NonGoalItems")
                .AsSingle()
                .Lazy();

            Container.BindFactory<Vector3, Transform, Sprite, CollectedItem, CollectedItem.Factory>()
                .FromComponentInNewPrefab(_collectedItemPrefab)
                .AsSingle()
                .Lazy();
        }

        private void BindMenus()
        {
            Container.BindFactory<Transform, float, WinMenu, WinMenu.Factory>()
                .FromComponentInNewPrefab(_winMenuPrefab)
                .AsSingle()
                .Lazy();

            Container.BindFactory<Transform, FailMenu, FailMenu.Factory>()
                .FromComponentInNewPrefab(_failMenuPrefab)
                .AsSingle()
                .Lazy();
        }

        private void BindBoosters()
        {
            Container.BindFactory<Fan, Fan.Factory>()
                .FromComponentInNewPrefab(_fanPrefab)
                .AsSingle()
                .Lazy();
        }
    }
}