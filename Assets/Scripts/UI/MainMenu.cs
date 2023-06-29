using Services;
using UnityEngine;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform _parentTransform;
    private LevelItem.Factory _levelItemFactory;

    [Inject]
    public void Construct(LevelItem.Factory levelItemFactory)
    {
        _levelItemFactory = levelItemFactory;
    }

    private void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            _levelItemFactory.Create(i, _parentTransform);
        }
    }
}
