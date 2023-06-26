using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _itemsSet1;
    //[SerializeField] private GameObject[] _itemsSet2;
    //[SerializeField] private GameObject[] _itemsSet3;

    public Sprite CollectedItemImage { get; private set; }

    [Inject]
    public void Construct(string incomingName)
    {
        name = incomingName;
    }
    private void Start()
    {
        foreach (var item in _itemsSet1)
        {
            if (item.name == name)
            {
                var newItem = Instantiate(item, transform);
                CollectedItemImage = newItem.GetComponent<Image>().sprite;
            }
        }
    }
    
    public class Factory : PlaceholderFactory<string, ItemController> { }
}