using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Gameplay.Services;

namespace Gameplay.UI
{
    public class CollectedItem : MonoBehaviour
    {
        public string Id { get; private set; }

        [Inject]
        public void Construct(Vector3 initialPosition, Transform targetTransform, Sprite itemSprite)
        {
            Id = itemSprite.name;
            transform.position = initialPosition;
            transform.SetParent(targetTransform);
            GetComponent<Image>().sprite = itemSprite;
        }

        private void Start()
        {
            GetComponent<MovingController>().Launch(transform.parent.position);
        }

        public class Factory : PlaceholderFactory<Vector3, Transform, Sprite, CollectedItem>
        {
        }
    }
}