using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _collectedItemPrefab;
    [SerializeField] private GameObject _goalSlotPrefab;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CollectController _collectController;
    [SerializeField] private GoalsController _goalsController;
    
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
    }

    private void BindFactories()
    {
        Container.BindFactory<string, ItemController, ItemController.Factory>()
            .FromComponentInNewPrefab(_itemPrefab)
            .AsSingle()
            .Lazy();
        
        Container.BindFactory<Sprite, Transform, int, GoalSlot, GoalSlot.Factory>()
            .FromComponentInNewPrefab(_goalSlotPrefab)
            .UnderTransformGroup("GoalSlot")
            .AsSingle()
            .Lazy();
        
        Container.BindFactory<string, CollectedItem, CollectedItem.Factory>()
            .FromComponentInNewPrefab(_collectedItemPrefab)
            .AsSingle()
            .Lazy();
    }
}
