using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.InputHandlers
{
    public class ClickHandler : MonoBehaviour
    {
        private const float HoverScaleMultiplier = 1.2f;
        private const float HoverDuration = 0.1f;

        private Camera _mainCamera;
        private CollectController _collectController;

        private Transform _currentHoveredObject;
        private Vector3 _originalScale;

        private float _hoverTimer;
        private bool _isHovering;

        [Inject]
        public void Construct(CollectController collectController)
        {
            _collectController = collectController;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleClick();
            HandleHovering();
        }

        private void HandleHovering()
        {
            var hoverRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(hoverRay, out var hoverHit);

            if (hoverHit.collider.transform.parent != null)
            {
                var newHoveredObject = hoverHit.collider.transform.parent.transform;

                if (newHoveredObject != _currentHoveredObject)
                {
                    if (_currentHoveredObject != null)
                    {
                        ResetHoverSize(_currentHoveredObject);
                    }

                    _currentHoveredObject = newHoveredObject;
                    _hoverTimer = 0f;
                    _isHovering = true;
                    _originalScale = _currentHoveredObject.localScale;
                }
            }

            else
            {
                if (_currentHoveredObject != null)
                {
                    ResetHoverSize(_currentHoveredObject);
                    _currentHoveredObject = null;
                }
            }

            if (_isHovering)
            {
                HoverSize(_currentHoveredObject);
            }
        }

        private void HandleClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                CheckClickedItem(ray);
            }
        }

        private void CheckClickedItem(Ray ray)
        {
            Physics.Raycast(ray, out var hit);

            if (hit.collider.GetComponent<MeshCollider>() &&
                hit.collider.GetComponentInParent<CollectableItem>())
            {
                var newPosition = _mainCamera.WorldToScreenPoint(hit.transform.position);
                var itemIcon = hit.collider.GetComponentInChildren<Image>().sprite;
                _collectController.CollectItem(newPosition, itemIcon);
                Destroy(hit.collider.gameObject);
            }
        }

        private void ResetHoverSize(Transform target)
        {
            if (target == null || !target.GetComponent<CollectableItem>())
            {
                return;
            }

            _isHovering = false;
            _hoverTimer = 0f;
            target.localScale = _originalScale;
        }

        private void HoverSize(Transform target)
        {
            if (target == null || !target.GetComponent<CollectableItem>())
            {
                return;
            }

            _hoverTimer += Time.deltaTime;
            var t = Mathf.Clamp01(_hoverTimer / HoverDuration);
            var scale = Mathf.Lerp(1f, HoverScaleMultiplier, t);
            target.localScale = _originalScale * scale;

            if (_hoverTimer >= HoverDuration)
            {
                _isHovering = false;
            }
        }
    }
}