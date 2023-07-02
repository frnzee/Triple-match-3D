using System.Collections.Generic;
using System.Linq;
using Gameplay.Services;
using Services.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class CollectableItem : MonoBehaviour, IPointerUpHandler
    {
        private const float HorizontalRandomModifier = 1.8f;
        private const float MinVerticalRandomModifier = 0.5f;
        private const float VerticalRandomModifier = 1.5f;
        
        [SerializeField] private List<GameObject> _items;
        
        private CollectController _collectController;
        private GameManager _gameManager;
        private AudioManager _audioManager;
        private string _id { get; set; }
        private Sprite _icon2D { get; set; }

        [Inject]
        public void Construct(string initName, Transform parentTransform, CollectController collectController,
            GameManager gameManager, AudioManager audioManager)
        {
            _id = initName;
            _collectController = collectController;
            _gameManager = gameManager;
            _audioManager = audioManager;
            
            HandleTransform(parentTransform);
        }

        private void HandleTransform(Transform parentTransform)
        {
            transform.position = new Vector3(Random.Range(-HorizontalRandomModifier, HorizontalRandomModifier),
                Random.Range(MinVerticalRandomModifier, VerticalRandomModifier),
                Random.Range(-HorizontalRandomModifier, HorizontalRandomModifier));
            transform.rotation = Random.rotation;
            transform.SetParent(parentTransform);
        }

        private void Start()
        {
            foreach (var item in _items)
            {
                if (item.name == _id)
                {
                    var newItem = Instantiate(item, transform);
                    _icon2D = newItem.GetComponent<Image>().sprite;
                }
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_gameManager.CurrentGameState == GameState.Game)
            {
                _audioManager.PlayItemClickSound();
                    
                _collectController.CollectItem(eventData.position, _icon2D);
                _gameManager.RemoveItem(this);
                Destroy(gameObject);
            }
        }

        public class Factory : PlaceholderFactory<string, Transform, CollectableItem>
        {
        }
    }
}