using System.Collections.Generic;
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
        private const float HorizontalRandomModifier = 2.5f;
        private const float MinVerticalRandomModifier = 0.5f;
        private const float VerticalRandomModifier = 1.5f;
        
        [SerializeField] private List<GameObject> _items;
        
        private CollectController _collectController;
        private GameManager _gameManager;
        private AudioManager _audioManager;
        public string Id { get; private set; }
        public Sprite Icon2D { get; private set; }

        [Inject]
        public void Construct(string incomingName, Transform parentTransform, CollectController collectController,
            GameManager gameManager, AudioManager audioManager)
        {
            Id = incomingName;
            transform.position = new Vector3(Random.Range(-HorizontalRandomModifier, HorizontalRandomModifier), 
                Random.Range(MinVerticalRandomModifier, VerticalRandomModifier), 
                Random.Range(-HorizontalRandomModifier, HorizontalRandomModifier));
            transform.rotation = Random.rotation;
            transform.SetParent(parentTransform);
            _collectController = collectController;
            _gameManager = gameManager;
            _audioManager = audioManager;
        }

        private void Start()
        {
            foreach (var item in _items)
            {
                if (item.name == Id)
                {
                    var newItem = Instantiate(item, transform);
                    Icon2D = newItem.GetComponent<Image>().sprite;
                }
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_gameManager.CurrentGameState == GameState.Game)
            {
                _audioManager.Play("ItemClick");
                    
                _collectController.CollectItem(eventData.position, Icon2D);
                _gameManager.RemoveItem(this);
                Destroy(gameObject);
            }
        }

        public class Factory : PlaceholderFactory<string, Transform, CollectableItem>
        {
        }
    }
}