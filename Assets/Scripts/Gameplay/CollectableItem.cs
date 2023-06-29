using System.Collections.Generic;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class CollectableItem : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private List<GameObject> _itemsSet1;
        [SerializeField] private List<GameObject> _itemsSet2;
        [SerializeField] private List<GameObject> _itemsSet3;

        private const float HoverScaleMultiplier = 1.1f;
        private const float HoverDuration = 0.2f;

        private Camera _mainCamera;
        private CollectController _collectController;
        private GameManager _gameManager;

        private Transform _currentHoveredObject;
        private Vector3 _originalScale;

        private float _hoverTimer;
        private bool _isHovering;
        public string Id { get; private set; }
        public Sprite Icon2D { get; private set; }

        [Inject]
        public void Construct(string incomingName, Transform parentTransform, CollectController collectController,
            GameManager gameManager)
        {
            Id = incomingName;
            transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0.5f, 1.5f), Random.Range(-2.5f, 2.5f));
            transform.rotation = Random.rotation;
            transform.SetParent(parentTransform);
            _collectController = collectController;
            _originalScale = transform.localScale;
            _gameManager = gameManager;
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

        private void Update()
        {
            if (_isHovering)
            {
                HoverSize();
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_gameManager.CurrentGameState == GameState.Game)
            {
                _collectController.CollectItem(eventData.position, Icon2D);
                _gameManager.RemoveItem(this);
                Destroy(gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
            HoverSize();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetHoverSize();
        }

        private void ResetHoverSize()
        {
            _isHovering = false;
            _hoverTimer = 0f;
            transform.localScale = _originalScale;
        }

        private void HoverSize()
        {
            _hoverTimer += Time.deltaTime;
            var time = Mathf.Clamp01(_hoverTimer / HoverDuration);
            var scale = Mathf.Lerp(1f, HoverScaleMultiplier, time);
            transform.localScale = _originalScale * scale;

            if (_hoverTimer >= HoverDuration)
            {
                _isHovering = false;
            }
        }

        public class Factory : PlaceholderFactory<string, Transform, CollectableItem>
        {
        }
    }
}