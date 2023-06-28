using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _itemsSet1;
        [SerializeField] private List<GameObject> _itemsSet2;
        [SerializeField] private List<GameObject> _itemsSet3;

        public string Id { get; private set; }
        public Sprite Icon2D { get; private set; }

        [Inject]
        public void Construct(string incomingName, Transform parentTransform)
        {
            Id = incomingName;
            transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0.5f, 1.5f), Random.Range(-2.5f, 2.5f));
            transform.rotation = Random.rotation;
            transform.SetParent(parentTransform);
        }

        private void Start()
        {
            foreach (var item in _itemsSet1)
            {
                if (item.name == Id)
                {
                    var newItem = Instantiate(item, transform);
                    Icon2D = newItem.GetComponent<Image>().sprite;
                }
            }
        }

        public class Factory : PlaceholderFactory<string, Transform, CollectableItem>
        {
        }
    }
}